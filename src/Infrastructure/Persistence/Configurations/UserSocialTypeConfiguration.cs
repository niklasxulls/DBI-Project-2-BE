using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace stackblob.Infrastructure.Persistence.Configurations;
public class UserSocialTypeConfiguration : IEntityTypeConfiguration<UserSocialType>
{
    public void Configure(EntityTypeBuilder<UserSocialType> builder)
    {
        builder.HasKey(us => new { us.UserId, us.SocialTypeId });
        builder.Property(us => us.Url).IsRequired().HasMaxLength(200);

        builder.HasOne(s => s.User)
               .WithMany(u => u.Socials)
               .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(s => s.SocialType)
               .WithMany(s => s.Users)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
