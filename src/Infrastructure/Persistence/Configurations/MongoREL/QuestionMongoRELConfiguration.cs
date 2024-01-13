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
            builder.HasKey(a => a.QuestionId);

            builder.ToCollection("QUESTION");

            builder.Property(a => a.QuestionId)
                    .HasValueGenerator<MongoDbValueGenerator>()
                    .HasConversion<MongoDbValueConverter>()
                    .ValueGeneratedNever()
                    .HasElementName("_id");

            builder.Property(a => a.CreatedById)
                    .HasConversion<MongoDbValueNullableConverter>();
        }
    }
}
