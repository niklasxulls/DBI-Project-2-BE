using stackblob.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using stackblob.Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using stackblob.Domain.Settings;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Reflection.Emit;
using stackblob.Domain.Entities.MongoREL;
using stackblob.Domain.Entities.MongoREL.Defaults;
using stackblob.Domain.Entities.SqlREL;
using stackblob.Domain.Entities.SqlREL.Defaults;
using System.Xml;
using stackblob.Infrastructure.Persistence.Configurations.MongoREL;
using stackblob.Infrastructure.Persistence.Configurations.SqlREL;
using stackblob.Domain.Entities.MongoFE;

namespace stackblob.Infrastructure.Persistence;

public class StackblobMongoRELDbContext : DbContext, IStackblobMongoRELDbContext
{
    private readonly IOptions<ConnectionStringOptions> _connectionStringOptions;
    private readonly ICurrentUserService _currentUser;

    public DbSet<TagMongoREL> TagsMongoREL { get; set; } = null!;
    public DbSet<QuestionMongoREL> QuestionsMongoREL { get; set; } = null!;
    public DbSet<AnswerMongoREL> AnswersMongoREL { get; set; } = null!;
    public DbSet<UserMongoREL> UsersMongoREL { get; set; }

    //public DbSet<QuestionMongoFE> QuestionsMongoFE { get; set; }


    public StackblobMongoRELDbContext(DbContextOptions<StackblobMongoRELDbContext> options, IOptions<ConnectionStringOptions> connectionStringOptions, ICurrentUserService currentUser) : base(options)

    {
        _connectionStringOptions = connectionStringOptions;
        _currentUser = currentUser;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntityMongoREL>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTimeUtil.Now();
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTimeUtil.Now();
                entry.State = EntityState.Modified;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        builder.ApplyConfiguration(new UserMongoRELConfiguration());
        builder.ApplyConfiguration(new QuestionMongoRELConfiguration());
        builder.ApplyConfiguration(new AnswerMongoRELConfiguration());
        builder.ApplyConfiguration(new TagMongoRELConfiguration());

        base.OnModelCreating(builder);
    }
}
