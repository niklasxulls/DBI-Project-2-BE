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

namespace stackblob.Infrastructure.Persistence.Configurations;
//{
//    public class MongoQuestionConfiguration : IEntityTypeConfiguration<MongoQuestion>
//    { 
//        public void Configure(EntityTypeBuilder<MongoQuestion> builder)
//        {
//            if(GlobalUtil.IsMongoDb)
//            {
//                builder.ToCollection("MONGO_QUESTION");

//                builder.HasKey(a => a.QuestionId);

//                builder.Property(a => a.QuestionId)
//                       .HasValueGenerator<MongoDbValueGenerator>()
//                       .HasConversion<MongoDbValueConverter>()
//                       .ValueGeneratedNever();

//                builder.Property(a => a.CreatedById)
//                       .HasConversion<MongoDbValueNullableConverter>();


//                //builder.HasOne(a => a.CreatedBy)
//                //       .WithMany(a => a.MongoQuestionsCreated)
//                //       .HasForeignKey(a => a.QuestionId);
//            } else
//            {
//                builder.HasKey(a => a.QuestionId);


//                builder.ToTable("MONGO_QUESTION");

//                builder.Property(u => u.QuestionId)
//                       .HasValueGenerator<SqlServerValueGenerator>()
//                       .ValueGeneratedNever();


//                builder.Ignore(a => a.Tags);
//            }
//        }
//    }
//}
