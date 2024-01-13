using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities.MongoREL;

public class TagMongoREL
{
    public string TagId { get; set; }
    public string Name { get; set; } = string.Empty;
}
