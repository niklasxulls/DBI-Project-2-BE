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
        builder.HasKey(a => a.AnswerId);

        builder.ToCollection("ANSWER");

        builder.Property(a => a.AnswerId)
                .HasValueGenerator<MongoDbValueGenerator>()
                .HasConversion<MongoDbValueConverter>()
                .ValueGeneratedNever()
                .HasElementName("_id");

        builder.Property(a => a.QuestionId)
                .HasConversion<MongoDbValueConverter>();

        builder.Property(a => a.CreatedById)
                .HasConversion<MongoDbValueNullableConverter>();

    }
}
