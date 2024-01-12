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
    public interface IStackblobSqlRELDbContext
    {
        DbSet<TagSqlREL> TagsSqlREL { get; set; }
        DbSet<QuestionSqlREL> QuestionsSqlREL { get; set; }
        DbSet<AnswerSqlREL> AnswersSqlREL { get; set; }
        DbSet<UserSqlREL> UsersSqlREL { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    }
}
