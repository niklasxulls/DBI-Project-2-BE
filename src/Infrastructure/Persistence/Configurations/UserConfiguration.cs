using stackblob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>

    
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.UserId).UseIdentityColumn();



        builder.Property(u => u.Firstname).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Lastname).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(64).IsFixedLength();
        builder.Property(u => u.Salt).IsRequired().HasMaxLength(32);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.Property(u => u.StatusText).HasMaxLength(500);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Ignore(u => u.Name);
        builder.Ignore(u => u.Reputation);

        builder.HasOne(u => u.Banner).WithOne(a => a.UserBannerPicture).HasForeignKey<User>(u => u.BannerId);
        builder.HasOne(u => u.ProfilePicture).WithOne(a => a.UserProfilePicture).HasForeignKey<User>(u => u.ProfilePictureId);


        builder.HasMany(u => u.RefreshTokens).WithOne(r => r.User);
        builder.HasMany(u => u.Socials).WithOne(r => r.User);
        builder.HasMany(u => u.LoginLocations).WithOne(r => r.User);
    }

}
