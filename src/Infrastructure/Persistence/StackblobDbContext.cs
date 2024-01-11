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

public class StackblobDbContext : DbContext, IStackblobDbContext
{
    private readonly IOptions<ConnectionStringOptions> _connectionStringOptions;
    private readonly ICurrentUserService _currentUser;

    public DbSet<TagMongoREL> TagsMongoREL { get; set; } = null!;
    public DbSet<QuestionMongoREL> QuestionsMongoREL { get; set; } = null!;
    public DbSet<AnswerMongoREL> AnswersMongoREL { get; set; } = null!;
    public DbSet<UserMongoREL> UsersMongoREL { get; set; }

    public DbSet<QuestionMongoFE> QuestionsMongoFE { get; set; }


    public DbSet<TagSqlREL> TagsSqlREL { get; set; }
    public DbSet<QuestionSqlREL> QuestionsSqlREL { get; set; }
    public DbSet<AnswerSqlREL> AnswersSqlREL { get; set; }
    public DbSet<UserSqlREL> UsersSqlREL { get; set; }


    public StackblobDbContext(DbContextOptions<StackblobDbContext> options, IOptions<ConnectionStringOptions> connectionStringOptions, ICurrentUserService currentUser) : base(options)

    {
        _connectionStringOptions = connectionStringOptions;
        //, ICurrentUserService currentUser
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


        if (!GlobalUtil.IsMongoDb)
        {
            builder.Ignore<QuestionMongoREL>();
            builder.Entity<QuestionMongoREL>().Metadata.SetIsTableExcludedFromMigrations(true);

            builder.Ignore<AnswerMongoREL>();
            builder.Entity<AnswerMongoREL>().Metadata.SetIsTableExcludedFromMigrations(true);

            builder.Ignore<TagMongoREL>();
            builder.Entity<TagMongoREL>().Metadata.SetIsTableExcludedFromMigrations(true);

            builder.Ignore<UserMongoREL>();
            builder.Entity<UserMongoREL>().ToTable(nameof(UserMongoREL), t => t.ExcludeFromMigrations());
            builder.Entity<UserMongoREL>().Metadata.SetIsTableExcludedFromMigrations(true);

            builder.Ignore<QuestionTagMongoREL>();
            builder.Entity<QuestionTagMongoREL>().ToTable(nameof(QuestionTagMongoREL), t => t.ExcludeFromMigrations());
            builder.Entity<QuestionTagMongoREL>().Metadata.SetIsTableExcludedFromMigrations(true);

            builder.Ignore<QuestionMongoFE>();
            builder.Entity<QuestionMongoFE>().ToTable(nameof(QuestionMongoFE), t => t.ExcludeFromMigrations());
            builder.Entity<QuestionMongoFE>().Metadata.SetIsTableExcludedFromMigrations(true);
        }
        else
        {
            builder.Ignore<QuestionSqlREL>();
            builder.Entity<QuestionSqlREL>().ToTable(nameof(QuestionSqlREL), t => t.ExcludeFromMigrations());

            builder.Ignore<AnswerSqlREL>();
            builder.Entity<AnswerSqlREL>().ToTable(nameof(AnswerSqlREL), t => t.ExcludeFromMigrations());

            builder.Ignore<TagSqlREL>();
            builder.Entity<TagSqlREL>().ToTable(nameof(TagSqlREL), t => t.ExcludeFromMigrations());

            builder.Ignore<UserSqlREL>();
            builder.Entity<UserSqlREL>().ToTable(nameof(UserSqlREL), t => t.ExcludeFromMigrations());

            builder.Ignore<QuestionTagSqlREL>();
            builder.Entity<QuestionTagSqlREL>().ToTable(nameof(QuestionTagSqlREL), t => t.ExcludeFromMigrations());
        }


        builder.ApplyConfigurationsFromAssembly(assembly);

        //if (GlobalUtil.IsMongoDb)
        //{
        //    builder.ApplyConfiguration(new QuestionMongoRELConfiguration());
        //    builder.ApplyConfiguration(new AnswerMongoRELConfiguration());
        //    builder.ApplyConfiguration(new TagMongoRELConfiguration());
        //    builder.ApplyConfiguration(new UserMongoRELConfiguration());
        //    builder.ApplyConfiguration(new QuestionTagMongoRELConfiguration());
        //}
        //else
        //{
        //    builder.ApplyConfiguration(new QuestionSqlRELConfiguration());
        //    builder.ApplyConfiguration(new AnswerSqlRELConfiguration());
        //    builder.ApplyConfiguration(new TagSqlRELConfiguration());
        //    builder.ApplyConfiguration(new UserSqlRELConfiguration());
        //    builder.ApplyConfiguration(new QuestionTagRELConfiguration());


            base.OnModelCreating(builder);
    }
}
