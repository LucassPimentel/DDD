using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Enums;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.ValueObjects;

namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Entities.Mapper
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
