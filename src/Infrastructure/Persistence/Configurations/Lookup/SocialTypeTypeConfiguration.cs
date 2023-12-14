using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class SocialTypeTypeConfiguration : BaseEnumConfiguration<SocialTypeType>, IEntityTypeConfiguration<SocialTypeType>
{
    public override void Configure(EntityTypeBuilder<SocialTypeType> builder)
    {
        base.Configure(builder);

        builder.HasKey(r => r.SocialTypeId);
        builder.Property(r => r.SocialTypeId).HasConversion<int>();
    }
}
