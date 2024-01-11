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
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    { 
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            if(GlobalUtil.IsMongoDb)
            {
                builder.ToCollection("QUESTION");

                builder.HasKey(a => a.QuestionId);

                builder.Property(a => a.QuestionId)
                       .HasValueGenerator<MongoDbValueGenerator>()
                       .HasConversion<MongoDbValueConverter>()
                       .ValueGeneratedNever();

                //builder.Property(a => a.CreatedById)
                //       .HasConversion<MongoDbValueNullableConverter>();
            }
            else
            {
                builder.ToTable("QUESTION");

                builder.Property(u => u.QuestionId)
                       .HasValueGenerator<SqlServerValueGenerator>()
                       .ValueGeneratedNever();

                builder.Property(r => r.Title).IsRequired().HasMaxLength(150);
                builder.Property(r => r.Description).IsRequired().HasMaxLength(10000);
            }

            //builder.HasMany(u => u.Answers)
            //       .WithOne(r => r.Question);

            //builder.HasOne(a => a.CreatedBy)
            //       .WithMany(a => a.QuestionsCreated)
            //       .HasForeignKey(a => a.CreatedById);
        }
    }
}
