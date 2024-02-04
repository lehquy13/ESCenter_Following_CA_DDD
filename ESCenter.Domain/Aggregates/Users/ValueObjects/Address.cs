using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Users.ValueObjects;

public class Address : ValueObject
{
    public string City { get; } = string.Empty;

    public string Country { get; } = string.Empty;

    public Address()
    {
    }

    public Address(string city, string country)
    {
        City = city;
        Country = country;
    }

    public override string ToString()
    {
        return $"City: {City}, Country: {Country}";
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Country;
    }

    public bool Match(string tutorParamsAddress)
    {
        return City.ToLower().Contains(tutorParamsAddress.ToLower()) ||
               Country.ToLower().Contains(tutorParamsAddress.ToLower());
    }
}