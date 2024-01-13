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

public class StackblobSqlRELDbContext : DbContext, IStackblobSqlRELDbContext
{
    private readonly IOptions<ConnectionStringOptions> _connectionStringOptions;
    private readonly ICurrentUserService _currentUser;

    public DbSet<TagSqlREL> TagsSqlREL { get; set; }
    public DbSet<QuestionSqlREL> QuestionsSqlREL { get; set; }
    public DbSet<AnswerSqlREL> AnswersSqlREL { get; set; }
    public DbSet<UserSqlREL> UsersSqlREL { get; set; }


    public StackblobSqlRELDbContext(DbContextOptions<StackblobSqlRELDbContext> options, IOptions<ConnectionStringOptions> connectionStringOptions, ICurrentUserService currentUser) : base(options)

    {
        _connectionStringOptions = connectionStringOptions;
        _currentUser = currentUser;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntitySqlREL>())
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

        builder.ApplyConfiguration(new UserSqlRELConfiguration());
        builder.ApplyConfiguration(new QuestionSqlRELConfiguration());
        builder.ApplyConfiguration(new AnswerSqlRELConfiguration());
        builder.ApplyConfiguration(new TagSqlRELConfiguration());
        builder.ApplyConfiguration(new QuestionTagSqlRELConfiguration());

        base.OnModelCreating(builder);
    }
}
