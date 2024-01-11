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
//    public class MongoAnswerConfiguration : IEntityTypeConfiguration<MongoAnswer>
//    { 
//        public void Configure(EntityTypeBuilder<MongoAnswer> builder)
//        {
//            if(GlobalUtil.IsMongoDb)
//            {
//                builder.ToCollection("MONGO_ANSWER");

//                builder.HasKey(a => a.AnswerId);

//                builder.Property(a => a.AnswerId)
//                       .HasValueGenerator<MongoDbValueGenerator>()
//                       .HasConversion<MongoDbValueConverter>()
//                       .ValueGeneratedNever();

//                builder.Property(a => a.CreatedById)
//                       .HasConversion<MongoDbValueNullableConverter>();

//                //builder.HasOne(a => a.CreatedBy)
//                //       .WithMany(a => a.MongoAnswersCreated)
//                //       .HasForeignKey(a => a.CreatedById);
//            } else
//            {
//                builder.HasKey(a => a.AnswerId);


//                builder.ToTable("MONGO_ANSWER");

//                builder.Property(u => u.AnswerId)
//                       .HasValueGenerator<SqlServerValueGenerator>()
//                       .ValueGeneratedNever();
//            }
//        }
//    }
//}
