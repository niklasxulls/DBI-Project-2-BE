using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.EntityFrameworkCore.Extensions;
using stackblob.Domain.Settings;
using stackblob.Infrastructure.Persistence.Configurations._Base;
using stackblob.Domain.Entities.SqlREL;

namespace stackblob.Infrastructure.Persistence.Configurations.SqlREL;

public class UserSqlRELConfiguration : IEntityTypeConfiguration<UserSqlREL>
{
    public void Configure(EntityTypeBuilder<UserSqlREL> builder)
    {
        builder.ToTable("USER");
        
        builder.HasKey(a => a.UserId);

        builder.Property(u => u.Firstname).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Lastname).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(64).IsFixedLength();
        builder.Property(u => u.Salt).IsRequired().HasMaxLength(32);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.Property(u => u.StatusText).HasMaxLength(500);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Ignore(u => u.Name);

        builder.HasMany(a => a.QuestionsCreated)
               .WithOne(a => a.CreatedBy);

        builder.HasMany(a => a.AnswersCreated)
               .WithOne(a => a.CreatedBy);
    }

}
