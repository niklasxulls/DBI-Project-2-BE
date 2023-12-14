using stackblob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Infrastructure.Persistence.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{ 
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(r => r.AnswerId);
        builder.Property(r => r.AnswerId).UseIdentityColumn();

        builder.Property(a => a.Title).HasMaxLength(50);
        builder.Property(a => a.Description).HasMaxLength(10000);


        builder.HasOne(v => v.CorrectAnswerQuestion)
               .WithOne(q => q.CorrectAnswer)
               .HasForeignKey<Question>(q => q.CorrectAnswerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Question)
               .WithMany(q => q.Answers)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.CreatedBy)
               .WithMany(u => u.Answers)
               .OnDelete(DeleteBehavior.SetNull);



        builder.HasMany(v => v.AnswerVotes).WithOne(q => q.Answer);
        builder.HasMany(v => v.Comments).WithOne(q => q.Answer);
        builder.HasMany(v => v.Attachments).WithOne(q => q.Answer);

    }
}
