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
using stackblob.Domain.Entities.MongoREL;

namespace stackblob.Infrastructure.Persistence.Configurations.MongoREL;

public class AnswerMongoRELConfiguration : IEntityTypeConfiguration<AnswerMongoREL>
{
    public void Configure(EntityTypeBuilder<AnswerMongoREL> builder)
    {
        if (!GlobalUtil.IsMongoDb)
        {
            builder.HasKey(a => a.AnswerId);

            //builder.Ignore(a => a.CreatedAt);
            //builder.Ignore(a => a.UpdatedAt);
            //builder.Ignore(a => a.Question);
            builder.Ignore(a => a.QuestionId);
            //builder.Ignore(a => a.CreatedBy);
            //builder.Ignore(a => a.CreatedById);
            builder.Ignore(a => a.Description);
            builder.Ignore(a => a.Title);

            return;
        }

        builder.HasKey(a => a.AnswerId);

        builder.ToCollection("ANSWER");

        builder.Property(a => a.AnswerId)
                .HasValueGenerator<MongoDbValueGenerator>()
                .HasConversion<MongoDbValueConverter>()
                .ValueGeneratedNever();

        builder.Property(a => a.QuestionId)
                .HasConversion<MongoDbValueConverter>();

        //builder.Property(a => a.CreatedById)
        //        .HasConversion<MongoDbValueNullableConverter>();

        //builder.HasOne(v => v.Question)
        //       .WithMany(q => q.Answers)
        //       .HasForeignKey(v => v.QuestionId);

        //builder.HasOne(a => a.CreatedBy)
        //       .WithMany(a => a.AnswersCreated)
        //       .HasForeignKey(a => a.CreatedById);
    }
}
