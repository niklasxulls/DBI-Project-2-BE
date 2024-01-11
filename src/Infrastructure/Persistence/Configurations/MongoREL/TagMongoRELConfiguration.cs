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
        if (!GlobalUtil.IsMongoDb)
        {
            builder.HasKey(r => r.TagId);

            builder.Ignore(a => a.Name);
            builder.Ignore(a => a.Questions);

            return;
        }

        builder.HasKey(r => r.TagId);

        builder.ToCollection("TAG");

        builder.HasKey(a => a.TagId);


        builder.Property(a => a.TagId)
                .HasValueGenerator<MongoDbValueGenerator>()
                .HasConversion<MongoDbValueConverter>()
                .ValueGeneratedNever();

        builder.HasMany(a => a.Questions)
               .WithOne(a => a.Tag)
               .HasForeignKey(a => a.TagId);
    }
}
