
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using stackblob.Domain.ValueObjects;

namespace stackblob.Application.Interfaces.Services;

public interface ICurrentUserService
{
    ObjectId? UserId { get; }
    bool IsVerified { get; }
    IpAddress IpAddress { get; }
}
