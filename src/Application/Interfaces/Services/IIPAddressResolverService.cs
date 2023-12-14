using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Domain.ValueObjects;
using stackblob.Infrastructure.Other;

namespace stackblob.Application.Interfaces.Services;
public interface IIPAddressResolverService
{

    public Task<IPAddressLocation> ResolveLocationDetailsByIPAddress(IpAddress ipAddress);
}
