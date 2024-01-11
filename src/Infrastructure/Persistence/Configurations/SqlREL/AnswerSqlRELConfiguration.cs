using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.EntityFrameworkCore.Extensions;
using stackblob.Domain.Settings;
using stackblob.Infrastructure.Persistence.Configurations._Base;
using stackblob.Domain.Entities.SqlREL;

namespace stackblob.Infrastructure.Persistence.Configurations.SqlREL;

public class AnswerSqlRELConfiguration : IEntityTypeConfiguration<AnswerSqlREL>
{
    public void Configure(EntityTypeBuilder<AnswerSqlREL> builder)
    {
        builder.ToTable("ANSWER");
        
        builder.HasKey(a => a.AnswerId);
        
        builder.Property(a => a.Title).HasMaxLength(50);
        builder.Property(a => a.Description).HasMaxLength(10000);


        builder.HasOne(v => v.Question)
               .WithMany(q => q.Answers)
               .HasForeignKey(v => v.QuestionId);

        builder.HasOne(a => a.CreatedBy)
               .WithMany(a => a.AnswersCreated)
               .HasForeignKey(a => a.CreatedById);
    }
}
