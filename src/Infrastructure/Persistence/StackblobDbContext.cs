using stackblob.Application.Interfaces;
using stackblob.Domain.Entities;
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

namespace stackblob.Infrastructure.Persistence;

public class StackblobDbContext : DbContext, IStackblobDbContext
{
    private readonly IOptions<ConnectionStringOptions> _connectionStringOptions;
    private readonly ICurrentUserService _currentUser;

    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<User> Users { get; set; }

    public StackblobDbContext(DbContextOptions<StackblobDbContext> options, IOptions<ConnectionStringOptions> connectionStringOptions, ICurrentUserService currentUser) : base(options)

    {
        _connectionStringOptions = connectionStringOptions;
        //, ICurrentUserService currentUser
        _currentUser = currentUser;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
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

        foreach (var entry in ChangeTracker.Entries<BaseEntityUserTracking>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedById = _currentUser.UserId;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        builder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(builder);
    }
}
