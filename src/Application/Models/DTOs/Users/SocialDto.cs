using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;

namespace stackblob.Application.Models.DTOs.Users;

public class SocialDto : IMapFrom<UserSocialType>
{
    public SocialType SocialTypeId { get; set; }
    public string Url { get; set; } = string.Empty;

}
