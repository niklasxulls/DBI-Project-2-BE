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

//public class QuestionMongoFEConfiguration : IEntityTypeConfiguration<QuestionMongoFE>
//{
    //public void Configure(EntityTypeBuilder<QuestionMongoFE> builder)
    //{
    //    if (!GlobalUtil.IsMongoDb)
    //    {
    //        builder.HasKey(a => a.QuestionId);

    //        //builder.Ignore(a => a.CreatedAt);
    //        //builder.Ignore(a => a.Answers);
    //        //builder.Ignore(a => a.UpdatedAt);
    //        //builder.Ignore(a => a.Tags);
    //        //builder.Ignore(a => a.CreatedBy);
    //        builder.Ignore(a => a.Description);
    //        builder.Ignore(a => a.Title);

    //        return;
    //    }

    //    builder.ToCollection("QUESTIONFE");

    //    builder.HasKey(a => a.QuestionId);


    //    builder.Property(a => a.QuestionId)
    //            .HasValueGenerator<MongoDbValueGenerator>()
    //            .HasConversion<MongoDbValueConverter>()
    //            .ValueGeneratedNever();

    //    //builder.OwnsOne(a => a.CreatedBy, o =>
    //    //{
    //    //    o.Property(z => z.UserId)
    //    //        .HasValueGenerator<MongoDbValueGenerator>()
    //    //        .HasConversion<MongoDbValueConverter>()
    //    //        .ValueGeneratedNever();
    //    //});

    //    //builder.OwnsMany(a => a.Answers, o =>
    //    //{
    //    //    o.Property(x => x.AnswerId)
    //    //        .HasValueGenerator<MongoDbValueGenerator>()
    //    //        .HasConversion<MongoDbValueConverter>()
    //    //        .ValueGeneratedNever();

    //    //    o.OwnsOne(a => a.CreatedBy, a =>
    //    //    {
    //    //        a.Property(c => c.UserId)
    //    //            .HasValueGenerator<MongoDbValueGenerator>()
    //    //            .HasConversion<MongoDbValueConverter>()
    //    //            .ValueGeneratedNever();
    //    //    });
    //    //});

    //    //builder.OwnsMany(a => a.Tags, o =>
    //    //{
    //    //    o.Property(c => c.TagId)
    //    //        .HasValueGenerator<MongoDbValueGenerator>()
    //    //        .HasConversion<MongoDbValueConverter>()
    //    //        .ValueGeneratedNever();
    //    //});
    //}
//}
