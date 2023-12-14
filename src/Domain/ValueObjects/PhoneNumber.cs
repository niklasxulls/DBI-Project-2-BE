using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    // add proper handling when saving the phone number, regarding splitting indents (etc.),
    // for spaces making it easy to differenciate between parts of the number (' ') add contstraints!
    public string CountryCode { get; set; } = string.Empty;
    public string AreaCode { get; set; } = string.Empty;
    public string DilingNumber { get; set; } = string.Empty;

    private PhoneNumber() { }

    private PhoneNumber(string? phoneNumber)
    {
        if(phoneNumber is null)
            throw new ArgumentException("Invalid Phone number!");

        if (phoneNumber[0] != '+')
            throw new ArgumentException("Invalid Phone number country code!");

        var splitNumber = phoneNumber.Split(" ");
        
        if (splitNumber.Length != 3) 
            throw new ArgumentException("Invalid Phone number length!");

        CountryCode = splitNumber[0];
        AreaCode = splitNumber[1];
        DilingNumber = splitNumber[2];
    }

    public static PhoneNumber From(string phoneNumber)
    {
        return new PhoneNumber(phoneNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToString();
    }

    public override string ToString()
    {
        return $"{CountryCode} {AreaCode} {DilingNumber}";
    }
}
