using stackblob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class UserEmailVerificationConfiguration : IEntityTypeConfiguration<UserEmailVerification>

{
    public void Configure(EntityTypeBuilder<UserEmailVerification> builder)
    {

        builder.HasKey(u => u.UserEmailVerificationId);
        builder.Property(u => u.UserEmailVerificationId).UseIdentityColumn();

        builder.Property(u => u.UserEmailVerificationAccess).ValueGeneratedOnAdd().HasValueGenerator<GuidValueGenerator>();

        builder.HasOne(u => u.User)
               .WithMany(u => u.EmailVerficiations);
        
    }

}
