using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate
{
    public class ShippingAddress : ValueObject
    {
        public string Street { get; private set; } = string.Empty;

        public string City { get; private set; } = string.Empty;

        public string State { get; private set; } = string.Empty;

        public string Country { get; private set; } = string.Empty;

        public string ZipCode { get; private set; } = string.Empty;

        public ShippingAddress() { }

        public ShippingAddress(string street, string city, string state, string country, string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }
    }
}
