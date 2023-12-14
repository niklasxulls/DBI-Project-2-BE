using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Domain.Settings;

namespace stackblob.Application.Models.DTOs.Users;
public class UserShallowReadDto : IMapFrom<User>
{
    public int UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    
    public void Mapping(Profile p)
    {
        p.CreateMap<User, UserShallowReadDto>()
            .ForMember(d => d.ProfilePictureUrl, s => s.MapFrom(src => src.ProfilePicture != null ? FileSettings.PublicFileBaseUrl + src.ProfilePicture.RelativePath : null))
            .ReverseMap();
        ;
    }

}
