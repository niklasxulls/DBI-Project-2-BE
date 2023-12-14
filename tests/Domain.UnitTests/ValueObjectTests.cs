using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Domain.ValueObjects;
using Xunit;

namespace stackblob.Domain.UnitTests;

public class ValueObjectTests
{

    public ValueObjectTests()
    {
    }

    // Email Tests

    [Fact]
    public void CheckEmailParts()
    {
        var emailValObject = Email.From("testname.test@gmail.com");

        Assert.Equal("testname.test", emailValObject.Name);
        Assert.Equal("gmail.com", emailValObject.Domain);
    }

    // IpAddress Tests

    [Fact]
    public void CheckIpAddress()
    {
        var ipAddressObject = IpAddress.From("127.0.0.1");

        Assert.Equal("127.0.0.1", ipAddressObject.Address);
    }

    [Fact]
    public void CheckIpAddressToString()
    {
        var shouldBe = "127.0.0.1";
        var ipAddressObject = IpAddress.From(shouldBe);

        Assert.Equal(shouldBe, ipAddressObject.ToString());
    }

    // Phone Number Tests

    [Fact]
    public void CheckPhoneNumberParts()
    {
        var phoneNumberObject = PhoneNumber.From("+43 680 1291323");

        Assert.Equal("+43", phoneNumberObject.CountryCode);
        Assert.Equal("680", phoneNumberObject.AreaCode);
        Assert.Equal("1291323", phoneNumberObject.DilingNumber);
    }

    [Fact]
    public void CheckToStringEqual()
    {
        var phoneNumberObject = PhoneNumber.From("+43 680 1291323");

        Assert.Equal("+43 680 1291323", phoneNumberObject.ToString());
    }

    [Fact]
    public void CheckIsFirstCharacterValid()
    {
        Assert.Throws<ArgumentException>(() => PhoneNumber.From("43 680 1291323"));
    }

    [Fact]
    public void CheckPhoneNullException()
    {
        Assert.Throws<ArgumentException>(() => PhoneNumber.From(null!)); 
    }

    [Fact]
    public void CheckPhoneLengthException()
    {
        Assert.Throws<ArgumentException>(() => PhoneNumber.From("+43 680"));
    }
}
