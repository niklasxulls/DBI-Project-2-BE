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

namespace stackblob.Infrastructure.Persistence;

public class StackblobDbContext : DbContext, IStackblobDbContext
{
    private readonly ICurrentUserService _currentUser;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<AttachmentTypeType> AttachmentTypes { get; set; } = null!;
    public DbSet<UserSocialType> UserSocialTypes { get; set; } = null!;
    public DbSet<SocialTypeType> SocialTypes { get; set; } = null!;
    public DbSet<RoleTypeType> RoleTypes { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Vote> Votes { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<LoginLocation> LoginLocations { get; set; } = null!;
    public DbSet<Domain.Entities.Lookup.TimeZone> TimeZones { get; set; } = null!;
    public DbSet<UserEmailVerification> UserEmailVerifications { get; set; }

    public StackblobDbContext(DbContextOptions<StackblobDbContext> options, ICurrentUserService currentUser) : base(options)

    {
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
                entry.Entity.CreatedById = _currentUser.UserId.GetValueOrDefault();
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
