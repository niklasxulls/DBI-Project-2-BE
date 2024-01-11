using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using stackblob.Domain.Settings;
using MongoDB.EntityFrameworkCore.Extensions;
using stackblob.Infrastructure.Persistence.Configurations._Base;
using stackblob.Domain.Entities.SqlREL;

namespace stackblob.Infrastructure.Persistence.Configurations.SqlREL;

public class QuestionSqlRELConfiguration : IEntityTypeConfiguration<QuestionSqlREL>
{
    public void Configure(EntityTypeBuilder<QuestionSqlREL> builder)
    {
        builder.ToTable("QUESTION");
        
        builder.HasKey(a => a.QuestionId);

        builder.Property(r => r.Title).IsRequired().HasMaxLength(150);
        builder.Property(r => r.Description).IsRequired().HasMaxLength(10000);

        builder.HasOne(a => a.CreatedBy)
               .WithMany(a => a.QuestionsCreated)
               .HasForeignKey(a => a.CreatedById);

        builder.HasMany(a => a.Tags)
               .WithOne(a => a.Question)
               .HasForeignKey(a => a.QuestionId);
    }
}
