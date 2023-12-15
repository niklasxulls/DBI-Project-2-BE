using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MongoDB.Bson;

namespace stackblob.Infrastructure.Persistence.Configurations._Base;

public class MongoDbValueGenerator : ValueGenerator<ObjectId>
{
    public override bool GeneratesTemporaryValues { get; }

    public override ObjectId Next(EntityEntry entry)
    {
        return ObjectId.GenerateNewId();
    }
}