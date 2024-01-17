using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using stackblob.Domain.Entities.MongoREL;
using stackblob.Domain.Entities.SqlREL;
using stackblob.Domain.Entities.MongoFE;

namespace stackblob.Application.Interfaces
{
    public interface IStackblobMongoRELDbContext
    {
        DbSet<TagMongoREL> TagsMongoREL { get; set; }
        DbSet<QuestionMongoREL> QuestionsMongoREL { get; set; }
        DbSet<AnswerMongoREL> AnswersMongoREL { get; set; }
        DbSet<UserMongoREL> UsersMongoREL { get; set; }



        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    }
}
