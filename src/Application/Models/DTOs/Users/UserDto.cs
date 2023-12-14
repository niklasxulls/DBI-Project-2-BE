using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Domain.Settings;

namespace stackblob.Application.Models.DTOs.Users;

public class UserDto : UserShallowReadDto, IMapFrom<User>
{
    public string? BannerUrl { get; set; }
    public string? StatusText { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public void Mapping(Profile p)
    {
        p.CreateMap<User, UserDto>()
            .IncludeBase<User, UserShallowReadDto>()
            .ReverseMap();
        ;
    }

}
