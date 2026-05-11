using ServiceOrder.Domain.Enums;
using ServiceOrder.Domain.ValueObjects;

namespace ServiceOrder.Application.Mapper
{
    public static class ServiceTypeMapper
    {
        public static ServiceType? ToEntity(this ServiceTypeEnum? serviceType)
        {
            return serviceType switch
            {
                ServiceTypeEnum.Normal => ServiceType.Normal,
                ServiceTypeEnum.Urgent => ServiceType.Urgent,
                _ => null
            };
        }
    }
}
