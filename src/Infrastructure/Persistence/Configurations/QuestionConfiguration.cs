using stackblob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace stackblob.Infrastructure.Persistence.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    { 
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(r => r.QuestionId);
            builder.Property(r => r.QuestionId).UseIdentityColumn();

            builder.HasIndex(r => r.QuestionIdAccess).IsUnique();
            builder.Property(r => r.QuestionIdAccess)
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator<GuidValueGenerator>();

            builder.Ignore(q => q.Popularity);

            builder.Property(r => r.Title).IsRequired().HasMaxLength(150);
            builder.Property(r => r.Description).IsRequired().HasMaxLength(10000);

            builder.HasOne(r => r.CorrectAnswer)
                   .WithOne(r => r.CorrectAnswerQuestion)
                   .HasForeignKey<Answer>(q => q.CorrectAnswerQuestionId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(r => r.CreatedBy)
                    .WithMany(r => r.QuestionsCreated)
                    .HasForeignKey(q => q.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(u => u.QuestionVotes).WithOne(r => r.Question);
            builder.HasMany(u => u.Tags).WithMany(r => r.Questions);
            builder.HasMany(u => u.Attachments).WithOne(r => r.Question);
            builder.HasMany(u => u.Answers).WithOne(r => r.Question);
        }
    }
}
