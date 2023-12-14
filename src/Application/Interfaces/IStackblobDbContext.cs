using stackblob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace stackblob.Application.Interfaces
{
    public interface IStackblobDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<UserEmailVerification> UserEmailVerifications { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<Attachment> Attachments { get; set; }
        DbSet<SocialTypeType> SocialTypes { get; set; }
        DbSet<RoleTypeType> RoleTypes { get; set; }
        DbSet<AttachmentTypeType> AttachmentTypes { get; set; }
        DbSet<UserSocialType> UserSocialTypes { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Vote> Votes { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<LoginLocation> LoginLocations { get; set; }
        DbSet<Domain.Entities.Lookup.TimeZone> TimeZones { get; set; }



        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    }
}
