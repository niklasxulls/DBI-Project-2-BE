using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Domain.Entities;
using stackblob.Domain.ValueObjects;
using Xunit;

namespace stackblob.Domain.UnitTests;

public class EntityTests
{

    public EntityTests()
    {
    }

    // Email Tests

    [Fact]
    public void CheckUser()
    {
        var userObject = new User()
        {

        };

        //Assert.Equal("testname", emailValObject.Name);
        //Assert.Equal("gmail.com", emailValObject.Domain);
    }
}
