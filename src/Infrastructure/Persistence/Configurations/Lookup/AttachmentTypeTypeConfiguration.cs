using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class AttachmentTypeTypeConfiguration : BaseEnumConfiguration<AttachmentTypeType>, IEntityTypeConfiguration<AttachmentTypeType>
{
    public override void Configure(EntityTypeBuilder<AttachmentTypeType> builder)
    {
        base.Configure(builder);

        builder.HasKey(r => r.AttachmentTypeId);
        builder.Property(r => r.AttachmentTypeId).HasConversion<int>();
    }
}
