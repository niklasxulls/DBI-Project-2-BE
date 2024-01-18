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
using stackblob.Domain.Entities.MongoFE;

namespace stackblob.Infrastructure.Persistence.Configurations.MongoFE;

public class QuestionMongoFEConfiguration : IEntityTypeConfiguration<QuestionMongoFE>
{
    public void Configure(EntityTypeBuilder<QuestionMongoFE> builder)
    {
        builder.ToCollection("QUESTIONFE");

        builder.HasKey(a => a._id);


        builder.Property(a => a._id)
                .HasValueGenerator<MongoDbValueGeneratorX>()
                .ValueGeneratedNever()
                .HasElementName("_id");


        //builder.OwnsOne(a => a.CreatedBy, o =>
        //{
        //    o.Property(z => z.UserId)
        //        .HasValueGenerator<MongoDbValueGenerator>()
        //        .HasConversion<MongoDbValueConverter>()
        //        .ValueGeneratedNever();
        //});

        //builder.OwnsMany(a => a.Answers, o =>
        //{
        //    o.Property(x => x.AnswerId)
        //        .HasValueGenerator<MongoDbValueGenerator>()
        //        .HasConversion<MongoDbValueConverter>()
        //        .ValueGeneratedNever();

        //    o.OwnsOne(a => a.CreatedBy, a =>
        //    {
        //        a.Property(c => c.UserId)
        //            .HasValueGenerator<MongoDbValueGenerator>()
        //            .HasConversion<MongoDbValueConverter>()
        //            .ValueGeneratedNever();
        //    });
        //});

        //builder.Ignore(a => a.Tags);
    }
}
