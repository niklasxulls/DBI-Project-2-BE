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
        if (GlobalUtil.IsMongoDb)
        {
            builder.ToCollection("ANSWER");

            builder.HasKey(a => a.AnswerId);

            builder.Property(a => a.AnswerId)
                   .HasValueGenerator<MongoDbValueGenerator>()
                   .HasConversion<MongoDbValueConverter>()
                   .ValueGeneratedNever();

            builder.Property(a => a.QuestionId)
                   .HasConversion<MongoDbValueConverter>();

        }
        else
        {
            builder.ToTable("ANSWER");


            builder.Property(a => a.Title).HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(10000);


            builder.Property(r => r.AnswerId).UseIdentityColumn();
        }

        //builder.HasOne(v => v.CorrectAnswerQuestion)
        //       .WithOne(q => q.CorrectAnswer)
        //       .HasForeignKey<Question>(q => q.CorrectAnswerId);

        builder.HasOne(v => v.Question)
               .WithMany(q => q.Answers);

        //builder.HasOne(v => v.CreatedBy)
        //       .WithMany(u => u.Answers);

        builder.HasKey(r => r.AnswerId);


    }
}
