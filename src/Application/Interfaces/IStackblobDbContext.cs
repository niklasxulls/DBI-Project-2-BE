using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using stackblob.Domain.Entities.MongoREL;
using stackblob.Domain.Entities.SqlREL;

namespace stackblob.Application.Interfaces
{
    public interface IStackblobDbContext
    {
        DbSet<TagMongoREL> TagsMongoREL { get; set; }
        DbSet<QuestionMongoREL> QuestionsMongoREL { get; set; }
        DbSet<AnswerMongoREL> AnswersMongoREL { get; set; }
        DbSet<UserMongoREL> UsersMongoREL { get; set; }


        DbSet<TagSqlREL> TagsSqlREL { get; set; }
        DbSet<QuestionSqlREL> QuestionsSqlREL { get; set; }
        DbSet<AnswerSqlREL> AnswersSqlREL { get; set; }
        DbSet<UserSqlREL> UsersSqlREL { get; set; }


        //DbSet<MongoQuestion> MongoQuestions { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    }
}
