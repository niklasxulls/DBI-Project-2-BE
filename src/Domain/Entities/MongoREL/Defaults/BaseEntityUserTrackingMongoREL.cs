using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using stackblob.Domain.Entities.MongoREL;

namespace stackblob.Domain.Entities.MongoREL.Defaults;

public class BaseEntityUserTrackingMongoREL : BaseEntityMongoREL
{
    public UserMongoREL? CreatedBy { get; set; } = null!;
    public string? CreatedById { get; set; }
}
