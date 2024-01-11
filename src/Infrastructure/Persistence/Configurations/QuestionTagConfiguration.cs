using stackblob.Domain.Entities;
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

namespace stackblob.Infrastructure.Persistence.Configurations
{
    public class QuestionTagConfiguration : IEntityTypeConfiguration<QuestionTag>
    { 
        public void Configure(EntityTypeBuilder<QuestionTag> builder)
        {
            builder.HasKey(a => new { a.QuestionId, a.TagId });

            if(GlobalUtil.IsMongoDb)
            {
                builder.ToCollection("QUESTION_TAG");

                builder.Property(a => a.QuestionId)
                       .HasConversion<MongoDbValueConverter>();

                builder.Property(a => a.TagId)
                       .HasConversion<MongoDbValueConverter>();
            }
            else
            {
                builder.ToTable("QUESTION_TAG");
            }


            //builder.HasOne(r => r.Question)
            //       .WithMany(r => r.Tags)
            //       .HasForeignKey(a => a.QuestionId);

            //builder.HasOne(r => r.Tag)
            //       .WithMany(r => r.Questions)
            //       .HasForeignKey(a => a.TagId);

        }
    }
}
