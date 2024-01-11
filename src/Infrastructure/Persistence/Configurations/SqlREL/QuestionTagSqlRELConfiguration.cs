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

public class QuestionTagSqlRELConfiguration : IEntityTypeConfiguration<QuestionTagSqlREL>
{
    public void Configure(EntityTypeBuilder<QuestionTagSqlREL> builder)
    {
        builder.HasKey(a => new { a.QuestionId, a.TagId });

        builder.ToTable("QUESTION_TAG");

        builder.HasOne(r => r.Question)
               .WithMany(r => r.Tags)
               .HasForeignKey(a => a.QuestionId);

        builder.HasOne(r => r.Tag)
               .WithMany(r => r.Questions)
               .HasForeignKey(a => a.TagId);
    }
}
