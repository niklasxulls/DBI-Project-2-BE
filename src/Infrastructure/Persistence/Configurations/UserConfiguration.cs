using stackblob.Domain.Entities;
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

namespace stackblob.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>

    
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(a => a.UserId);

        if (GlobalUtil.IsMongoDb)
        {
            builder.ToCollection("USER");

            builder.Property(a => a.UserId)
                   .HasValueGenerator<MongoDbValueGenerator>()
                   .HasConversion<MongoDbValueConverter>()
                   .ValueGeneratedNever();
        }
        else
        {
            builder.ToTable("USER");

            builder.Property(u => u.UserId)
                   .HasValueGenerator<SqlServerValueGenerator>()
                   .ValueGeneratedNever();

            builder.Property(u => u.Firstname).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Lastname).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(64).IsFixedLength();
            builder.Property(u => u.Salt).IsRequired().HasMaxLength(32);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
            builder.Property(u => u.StatusText).HasMaxLength(500);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Ignore(u => u.Name);
        }
    }

}
