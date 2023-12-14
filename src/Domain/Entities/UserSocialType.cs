using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class UserSocialType : BaseEntity
{
    public User User { get; set; } = null!;
    public int UserId { get; set; }
    public SocialType SocialTypeId { get; set;}
    public SocialTypeType SocialType { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
}
