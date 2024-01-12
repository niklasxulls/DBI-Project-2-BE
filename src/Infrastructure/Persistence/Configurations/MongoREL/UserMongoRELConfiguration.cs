using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

public class UserMongoRELConfiguration : IEntityTypeConfiguration<UserMongoREL>
{
    public void Configure(EntityTypeBuilder<UserMongoREL> builder)
    {
        if (!GlobalUtil.IsMongoDb)
        {
            builder.HasKey(a => a.UserId);

            //builder.Ignore(a => a.AnswersCreated);
            //builder.Ignore(a => a.QuestionsCreated);
            builder.Ignore(a => a.CreatedAt);
            builder.Ignore(a => a.Email);
            builder.Ignore(a => a.Firstname);
            builder.Ignore(a => a.Lastname);
            builder.Ignore(a => a.Name);
            builder.Ignore(a => a.Password);
            builder.Ignore(a => a.Salt);
            builder.Ignore(a => a.StatusText);
            builder.Ignore(a => a.UpdatedAt);


            return;
        }

        builder.HasKey(a => a.UserId);

        builder.ToCollection("USER");

        builder.Property(a => a.UserId)
                .HasValueGenerator<MongoDbValueGenerator>()
                .HasConversion<MongoDbValueConverter>()
                .ValueGeneratedNever();

        //builder.HasMany(a => a.QuestionsCreated)
        //       .WithOne(a => a.CreatedBy);

        //builder.HasMany(a => a.AnswersCreated)
        //       .WithOne(a => a.CreatedBy);
    }

}
