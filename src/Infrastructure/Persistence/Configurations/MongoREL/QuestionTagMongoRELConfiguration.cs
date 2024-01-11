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
    public class QuestionTagMongoRELConfiguration : IEntityTypeConfiguration<QuestionTagMongoREL>
    {
        public void Configure(EntityTypeBuilder<QuestionTagMongoREL> builder)
        {
            if (!GlobalUtil.IsMongoDb)
            {
                builder.HasKey(a => new { a.QuestionId, a.TagId });

                builder.Ignore(a => a.Question);
                builder.Ignore(a => a.Tag);

                return;
            }

            builder.HasKey(a => new { a.QuestionId, a.TagId });

            builder.ToCollection("QUESTION_TAG");

            builder.Property(a => a.QuestionId)
                    .HasConversion<MongoDbValueConverter>();

            builder.Property(a => a.TagId)
                    .HasConversion<MongoDbValueConverter>();


            builder.HasOne(r => r.Question)
                   .WithMany(r => r.Tags)
                   .HasForeignKey(a => a.QuestionId);

            builder.HasOne(r => r.Tag)
                   .WithMany(r => r.Questions)
                   .HasForeignKey(a => a.TagId);
        }
    }
}
