using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.ValueObjects;

namespace DDD.ServiceOrder.Api.Application.InputModels
{
    public class ServiceOrderAddressInputModel
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public ServiceOrderAddress ToValueObject()
        {
            return new ServiceOrderAddress(Street,
                Number,
                ZipCode,
                City,
                State,
                Country);
        }
    }
}
