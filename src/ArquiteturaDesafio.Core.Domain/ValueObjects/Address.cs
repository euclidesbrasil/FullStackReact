using ArquiteturaDesafio.Core.Domain.Common;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public Address() { }
        public Address(string city, string street, int number, string zipCode, Geolocation geolocation)
        {
            City = city;
            Street = street;
            Number = number;
            ZipCode = zipCode;
            Geolocation = geolocation;
        }

        public string City { get; private set; }
        public string Street { get; private set; }
        public int Number { get; private set; }
        public string ZipCode { get; private set; }
        public Geolocation Geolocation { get; private set; }
       
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return Street;
            yield return Number;
            yield return ZipCode;
            yield return Geolocation;
        }
    }
}
