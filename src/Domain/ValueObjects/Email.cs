using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.ValueObjects;

public class Email : ValueObject
{
    public string Name { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;

    private Email() { }   

    private Email(string email)
    {
        var emailSplit = email.Split("@");

        Name = emailSplit[0];
        Domain = emailSplit[1];
    }

    public static Email From(string email)
    {
        return new Email(email);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToString();
    }

    public override string ToString()
    {
        return $"{Name}@{Domain}";
    }
}
