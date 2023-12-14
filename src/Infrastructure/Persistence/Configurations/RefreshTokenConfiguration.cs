using stackblob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    { 
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Token);
            builder.Property(r => r.Token).HasMaxLength(64).IsFixedLength();
            builder.Property(r => r.ExpiresAt).IsRequired();

            builder.HasOne(u => u.User)
                   .WithMany(r => r.RefreshTokens)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
