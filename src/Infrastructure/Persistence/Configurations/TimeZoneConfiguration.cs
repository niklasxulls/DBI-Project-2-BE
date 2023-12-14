using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeZone = stackblob.Domain.Entities.Lookup.TimeZone;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class TimeZoneConfiguration : IEntityTypeConfiguration<TimeZone>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Lookup.TimeZone> builder)
    {
        builder.HasKey(u => u.TimeZoneId);
        builder.Property(u => u.TimeZoneId).UseIdentityColumn();

        builder.Property(u => u.Description).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Offset).IsRequired();
        //builder.Property(u => u.Latitude).IsRequired();
        //builder.Property(u => u.Longitude).IsRequired();

        builder.HasMany(u => u.LoginLocations).WithOne(t => t.TimeZone);

    }
}
