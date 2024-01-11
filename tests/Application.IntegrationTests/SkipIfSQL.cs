using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Domain.Settings;
using Xunit;

namespace stackblob.stackblob.Application.IntegrationTests;
public class SkipIfSQL : TheoryAttribute
{
    public SkipIfSQL()
    {
        if(!GlobalUtil.IsMongoDb)
        {
            Skip = "Skipped since IsSQL";
        }
    }
}
