using ServiceOrder.Application.InputModels;
using ServiceOrder.Application.ViewModels;
using ServiceOrder.Application.Services.Interfaces;
using ServiceOrder.Application.Extensions;
using ServiceOrder.Application.Mapper;
using ServiceOrder.Domain.Notification;
using ServiceOrder.Domain.Interfaces.Repositories;
using ServiceOrder.Application.Interfaces.EventBus;
using ServiceOrder.Domain.Events;

namespace ServiceOrder.Application.Services
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IEventBusRabbitMq _eventBusRabbitMq;
        public ServiceOrderService(IServiceOrderRepository serviceOrderRepository, ITechnicianRepository technicianRepository, NotificationContext notificationContext, IEventBusRabbitMq eventBusRabbitMq)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _technicianRepository = technicianRepository;
            _notificationContext = notificationContext;
            _eventBusRabbitMq = eventBusRabbitMq;
        }

        public async Task<IEnumerable<ServiceOrderViewModel>?> GetAllWithTechnicianAsync()
        {
            var serviceOrders = await _serviceOrderRepository.GetAllAsync();

            if (!serviceOrders.Any())
            {
                return [];
            }

            var technicianIds = serviceOrders
                .Where(x => x.TechnicianId.HasValue)
                .Select(x => x.TechnicianId!.Value)
                .Distinct()
                .ToList();

            var techniciansDict = (await _technicianRepository
                .GetByIdsAsync(technicianIds))
                .ToDictionary(t => t.Id, t => t);

            return serviceOrders
                    .Select(so => ServiceOrderMapper.ToViewModel(so, techniciansDict.FirstOrDefault(x => x.Key == so.TechnicianId).Value))
                    .ToList();
        }

        public async Task<ServiceOrderViewModel?> GetByIdAsync(int id)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
            if (serviceOrder == null)
                return null;

            var technician = serviceOrder.HasTechnician() ? await _technicianRepository.GetByIdAsync(serviceOrder.TechnicianId!.Value) : null;

            return ServiceOrderMapper.ToViewModel(serviceOrder, technician);
        }

        public async Task<int?> AddAsync(ServiceOrderInputModel serviceOrder)
        {
            var serviceOrderEntity = ServiceOrderMapper.ToEntity(serviceOrder);

            if (serviceOrderEntity.Invalid)
            {
                _notificationContext.AddNotifications(serviceOrderEntity.ValidationResult);
                return null;
            }

            var id = await _serviceOrderRepository.AddAsync(serviceOrderEntity);
            await _serviceOrderRepository.SaveChangesAsync();

            return id;
        }

        public async Task InitializeAsync(InitializeServiceInput initializeServiceInput)
        {
            var serviceOrderToInitialize = await _serviceOrderRepository.GetByIdAsync(initializeServiceInput.ServiceOrderId);

            if (_notificationContext.NotifyIfNull(serviceOrderToInitialize, "ServiceOrder", "Serviço não encontrado."))
            {
                return;
            }

            var technician = await _technicianRepository.GetByIdAsync(initializeServiceInput.TechnicianId);

            if (_notificationContext.NotifyIfNull(technician, "Technician", "Técnico não encontrado."))
            {
                return;
            }

            serviceOrderToInitialize!.AssignTechnician(initializeServiceInput.TechnicianId);

            if (!serviceOrderToInitialize.SetInitialized())
            {
                _notificationContext.AddNotifications(serviceOrderToInitialize.ValidationResult);
                return;
            }

            _serviceOrderRepository.Update(serviceOrderToInitialize);

            await _serviceOrderRepository.SaveChangesAsync();

            // publica evento para notificar o técnico que o serviço foi inicializado
            await _eventBusRabbitMq.PublishAsync(
                new ServiceOrderInitialized(
                    technician!.Email,
                    serviceOrderToInitialize.ServiceType.Name,
                    serviceOrderToInitialize.Id,
                    technician.Name));
        }
    }
}
