using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Application.Mapping;
using stackblob.Application.Models.DTOs.Users;

namespace stackblob.Application.Models.DTOs.Auth;
public class UserEmailVerificationDto : IMapFrom<UserEmailVerification>
{
    public int UserEmailVerificationId { get; set; }
    public UserDto User { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
