using Microservices.Services.Order.Domain.Core;

namespace Microservices.Services.Order.Domain.OrderAggregate;

public class Address : ValueObject
{
    #region Properties
    public string Province { get; private set; }
    public string District { get; private set; }
    public string Street { get; private set; }
    public string ZipCode { get; private set; }
    public string Line { get; private set; }

    #endregion

    #region ctor

    public Address(string province, string district, string street, string zipCode, string line)
    {
        Province = province;
        District = district;
        Street = street;
        ZipCode = zipCode;
        Line = line;
    }

    #endregion

    #region Methods
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Province;
        yield return District;
        yield return Street;
        yield return ZipCode;
        yield return Line;
    }

    #endregion

}