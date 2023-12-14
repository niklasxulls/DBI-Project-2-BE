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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    { 
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(r => r.CommentId);
            builder.Property(r => r.CommentId).UseIdentityColumn();

            builder.Property(r => r.Description).HasMaxLength(250);
                
            builder.HasOne(v => v.Answer)
                   .WithMany(q => q.Comments)
                   .HasForeignKey(c => c.AnswerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.CreatedByInAnswer)
                   .WithMany(u => u.AnswerComments) 
                   .HasForeignKey(c => c.CreatedByInAnswerId)
                   .OnDelete(DeleteBehavior.Restrict);
                   

            builder.HasOne(v => v.CreatedByInQuestion)
                   .WithMany(u => u.QuestionComments)
                   .HasForeignKey(c => c.CreatedByInQuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
