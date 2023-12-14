using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(r => r.TagId);
        builder.Property(r => r.TagId).HasConversion<int>();
        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();


        builder.HasMany(r => r.Questions)
               .WithMany(r => r.Tags);
    }
}
