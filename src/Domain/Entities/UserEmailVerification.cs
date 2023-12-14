using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;
public class UserEmailVerification : BaseEntity
{
    public int UserEmailVerificationId { get; set; }
    public Guid UserEmailVerificationAccess { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public bool IsVerified { get; set; }
    public DateTime ExpiresAt { get; set; }
}
