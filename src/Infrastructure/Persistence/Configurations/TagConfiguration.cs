using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using stackblob.Domain.Settings;
using stackblob.Infrastructure.Persistence.Configurations._Base;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(r => r.TagId);

        if (GlobalUtil.IsMongoDb)
        {
            builder.ToCollection("TAG");

            builder.HasKey(a => a.TagId);


            builder.Property(a => a.TagId)
                   .HasValueGenerator<MongoDbValueGenerator>()
                   .HasConversion<MongoDbValueConverter>()
                   .ValueGeneratedNever();

        }
        else
        {
            builder.ToTable("TAG");

            builder.Property(u => u.TagId)
                   .HasValueGenerator<SqlServerValueGenerator>()
                   .ValueGeneratedNever();

            builder.Property(r => r.Name).HasMaxLength(50).IsRequired();
        }
    }
}
