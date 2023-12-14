using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeZone = stackblob.Domain.Entities.Lookup.TimeZone;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class LoginLocationConfiguration : IEntityTypeConfiguration<LoginLocation>
{
    public void Configure(EntityTypeBuilder<LoginLocation> builder)
    {
        builder.HasKey(u => u.LoginLocationId);
        builder.Property(u => u.LoginLocationId).UseIdentityColumn();


        builder.Property(u => u.Latitude).IsRequired();
        builder.Property(u => u.Longitude).IsRequired();

        builder.OwnsOne(u => u.IpAddress, navigationBuilder =>
        {
            navigationBuilder.Property(ip => ip.Address).HasColumnName("IpAddress").HasMaxLength(36).IsRequired();
        });
            
        builder.HasOne(l => l.User)
               .WithMany(u => u.LoginLocations)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Country)
               .WithMany(c => c.LoginLocations)
               .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(l => l.TimeZone)
               .WithMany(t => t.LoginLocations)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
