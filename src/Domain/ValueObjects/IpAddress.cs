using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.ValueObjects;

public class IpAddress : ValueObject
{
    public string Address { get; set; } = string.Empty;

    private IpAddress() { }

    private IpAddress(string address)
    {
        Address = address;
    }

    public static IpAddress From(string address)
    {
        return new IpAddress(address);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }

    public override string ToString()
    {
        return Address;
    }
}