using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using stackblob.Domain.Entities.MongoREL;
using stackblob.Domain.Settings;
using stackblob.Infrastructure.Persistence.Configurations._Base;

namespace stackblob.Infrastructure.Persistence.Configurations.MongoREL;

public class TagMongoRELConfiguration : IEntityTypeConfiguration<TagMongoREL>
{
    public void Configure(EntityTypeBuilder<TagMongoREL> builder)
    {
        builder.ToCollection("TAG");

        builder.HasKey(a => a.TagId);

        builder.Property(a => a.TagId)
                .HasValueGenerator<MongoDbValueGenerator>()
                .HasConversion<MongoDbValueConverter>()
                .ValueGeneratedNever()
                .HasElementName("_id");
    }
}
