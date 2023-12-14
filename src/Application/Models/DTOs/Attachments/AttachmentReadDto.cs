using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Domain.Settings;

namespace stackblob.Application.Models.DTOs.Attachments;
public class AttachmentReadDto : AttachmentBaseDto, IMapFrom<Attachment>
{
    public int AttachmentId { get; set; }
    public string Url { get; set; } = string.Empty;
    public void Mapping(Profile p)
    {
        p.CreateMap<Attachment, AttachmentReadDto>()
            .ForMember(s => s.Url, o => o.MapFrom(src => FileSettings.PublicFileBaseUrl + src.RelativePath))
            .ReverseMap();
    }
}
