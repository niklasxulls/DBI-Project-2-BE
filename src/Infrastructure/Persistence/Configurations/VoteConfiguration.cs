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
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    { 
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasKey(r => r.VoteId);
            builder.Property(r => r.VoteId).UseIdentityColumn();

            builder.HasOne(v => v.Answer)
                   .WithMany(a => a.AnswerVotes)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.CreateByInAnswer)
                   .WithMany(u => u.AnswerVotes)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(v => v.CreateByInQuestion)
                   .WithMany(u => u.QuestionVotes)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
