using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.Property(a => a.AttachmentId).UseIdentityColumn();
        builder.HasKey(a => a.AttachmentId);

        builder.Property(a => a.Name).IsRequired().HasMaxLength(60);
        builder.Property(a => a.RelativePath).IsRequired().HasMaxLength(150);

        //fs part shouldn't be negleted and therefore deletion of attachments should happen in coherence

        builder.HasOne(u => u.UserBannerPicture).WithOne(a => a.Banner).HasForeignKey<Attachment>(u => u.UserBannerPictureId);
        builder.HasOne(u => u.UserProfilePicture).WithOne(a => a.ProfilePicture).HasForeignKey<Attachment>(u => u.UserProfilePictureId);

        builder.HasOne(a => a.Answer)
               .WithMany(a => a.Attachments)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
