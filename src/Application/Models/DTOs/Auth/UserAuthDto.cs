using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Application.Models.DTOs.Users;
using stackblob.Domain.Entities;
using stackblob.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Models.DTOs.Auth;

public class UserAuthDto : UserDto, IMapFrom<User>
{
    public UserAuthDto()
    {
    }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsVerified { get; set; }

    public new void Mapping(Profile p)
    {
        p.CreateMap<User, UserAuthDto>()
            .IncludeBase<User, UserDto>()
            .ReverseMap();
    }
}
