using DDD.ServiceOrder.Api.Application.InputModels;
using DDD.ServiceOrder.Api.Application.Services.Interfaces;
using DDD.ServiceOrder.Api.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DDD.ServiceOrder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceOrderController : ControllerBase
    {
        private readonly IServiceOrderService _serviceOrderService;

        public ServiceOrderController(IServiceOrderService serviceOrderService)
        {
            _serviceOrderService = serviceOrderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceOrderViewModel>>> GetAll()
        {
            var serviceOrder = await _serviceOrderService.GetAllWithTechnicianAsync();

            return Ok(serviceOrder);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceOrderViewModel>> GetById(int id)
        {
            var serviceOrder = await _serviceOrderService.GetByIdAsync(id);
            if (serviceOrder == null)
            {
                return NotFound();
            }
            return Ok(serviceOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ServiceOrderInputModel serviceOrder)
        {
            var newServiceOrderId = await _serviceOrderService.AddAsync(serviceOrder);
            return Created($"api/serviceorder/{newServiceOrderId}", newServiceOrderId);
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeAsync(InitializeServiceInput initializeServiceInput)
        {
            await _serviceOrderService.InitializeAsync(initializeServiceInput);
            return NoContent();
        }
    }
}
