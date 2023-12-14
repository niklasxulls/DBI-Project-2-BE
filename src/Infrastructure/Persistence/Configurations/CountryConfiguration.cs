using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(u => u.CountryId);
        builder.Property(u => u.CountryId).UseIdentityColumn();

        builder.Property(u => u.Abbreviation).IsRequired().HasMaxLength(5);
        builder.Property(u => u.CountryName).IsRequired().HasMaxLength(50);

        builder.HasMany(u => u.LoginLocations).WithOne(l => l.Country);

    }
}
