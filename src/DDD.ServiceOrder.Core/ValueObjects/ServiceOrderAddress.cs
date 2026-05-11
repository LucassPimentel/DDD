using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Entities;
using FluentValidation;

namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.ValueObjects
{

    public record ServiceOrderAddress : ValueObjectBase
    {
        public string Street { get; init; }
        public string Number { get; init; }
        public string ZipCode { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string Country { get; init; }

        private ServiceOrderAddress() { }

        public ServiceOrderAddress(string street, string number, string zipCode, string city, string state, string country)
        {
            Street = street;
            Number = number;
            ZipCode = zipCode;
            City = city;
            State = state;
            Country = country;

            Validate(this, new ServiceOrderAddressValidator());
        }
    }

    public class ServiceOrderAddressValidator : AbstractValidator<ServiceOrderAddress>
    {
        public ServiceOrderAddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty().WithMessage("A Rua é obrigatória.");
            RuleFor(x => x.Number).NotEmpty().WithMessage("O número é obrigatório.");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("O CEP é obrigatório.");
            RuleFor(x => x.City).NotEmpty().WithMessage("A Cidade é obrigatória.");
            RuleFor(x => x.State).NotEmpty().WithMessage("O Estado é obrigatório.");
            RuleFor(x => x.Country).NotEmpty().WithMessage("O País é obrigatório.");
        }
    }
}