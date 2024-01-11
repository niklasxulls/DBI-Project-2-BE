using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using stackblob.Domain.Entities.SqlREL;
using stackblob.Domain.Settings;
using stackblob.Infrastructure.Persistence.Configurations._Base;

namespace stackblob.Infrastructure.Persistence.Configurations.SqlREL;

public class TagSqlRELConfiguration : IEntityTypeConfiguration<TagSqlREL>
{
    public void Configure(EntityTypeBuilder<TagSqlREL> builder)
    {
        builder.ToTable("TAG");
        
        builder.HasKey(r => r.TagId);

        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();

        builder.HasMany(a => a.Questions)
               .WithOne(a => a.Tag)
               .HasForeignKey(a => a.TagId);
    }
}
