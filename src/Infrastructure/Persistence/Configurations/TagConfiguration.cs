using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using stackblob.Domain.Settings;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        if (GlobalUtil.IsMongoDb)
        {
            builder.ToCollection("TAG");
        }
        else
        {
            builder.ToTable("TAG");
        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();


        builder.HasMany(r => r.Questions)
               .WithMany(r => r.Tags);
        builder.Property(r => r.TagId).HasConversion<int>();
        }

        builder.HasKey(r => r.TagId);
    }
}
