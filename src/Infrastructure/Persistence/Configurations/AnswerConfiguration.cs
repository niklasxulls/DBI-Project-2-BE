using stackblob.Domain.Entities;
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

namespace stackblob.Infrastructure.Persistence.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{ 
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(a => a.AnswerId);

        if (GlobalUtil.IsMongoDb)
        {
            builder.ToCollection("ANSWER");

            builder.Property(a => a.AnswerId)
                   .HasValueGenerator<MongoDbValueGenerator>()
                   .HasConversion<MongoDbValueConverter>()
                   .ValueGeneratedNever();

            builder.Property(a => a.QuestionId)
                   .HasConversion<MongoDbValueConverter>();

            //builder.Property(a => a.CreatedById)
            //       .HasConversion<MongoDbValueNullableConverter>();
        }
        else
        {
            builder.ToTable("ANSWER");

            builder.Property(u => u.AnswerId)
                   .HasValueGenerator<SqlServerValueGenerator>()
                   .ValueGeneratedNever();

            builder.Property(a => a.Title).HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(10000);
        }


        builder.HasOne(v => v.Question)
               .WithMany(q => q.Answers)
               .HasForeignKey(v => v.QuestionId);

        //builder.HasOne(a => a.CreatedBy)
        //    .WithMany(a => a.AnswersCreated)
        //    .HasForeignKey(a => a.CreatedById);
    }
}
