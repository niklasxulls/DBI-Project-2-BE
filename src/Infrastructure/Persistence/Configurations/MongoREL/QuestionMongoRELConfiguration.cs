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
using stackblob.Domain.Entities.MongoREL;

namespace stackblob.Infrastructure.Persistence.Configurations.MongoREL
{
    public class QuestionMongoRELConfiguration : IEntityTypeConfiguration<QuestionMongoREL>
    {
        public void Configure(EntityTypeBuilder<QuestionMongoREL> builder)
        {
            if (!GlobalUtil.IsMongoDb)
            {
                builder.HasKey(a => a.QuestionId);

                //builder.Ignore(a => a.CreatedAt);
                //builder.Ignore(a => a.Answers);
                //builder.Ignore(a => a.UpdatedAt);
                //builder.Ignore(a => a.Tags);
                //builder.Ignore(a => a.CreatedBy);
                //builder.Ignore(a => a.CreatedById);
                builder.Ignore(a => a.Description);
                builder.Ignore(a => a.Title);

                return;
            }

            builder.HasKey(a => a.QuestionId);

            builder.ToCollection("QUESTION");

            builder.Property(a => a.QuestionId)
                    .HasValueGenerator<MongoDbValueGenerator>()
                    .HasConversion<MongoDbValueConverter>()
                    .ValueGeneratedNever();

            //builder.Property(a => a.CreatedById)
            //        .HasConversion<MongoDbValueNullableConverter>();

            //builder.HasOne(a => a.CreatedBy)
            //       .WithMany(a => a.QuestionsCreated)
            //       .HasForeignKey(a => a.CreatedById);

            //builder.HasMany(a => a.Tags)
            //       .WithOne(a => a.Question)
            //       .HasForeignKey(a => a.QuestionId);
        }
    }
}
