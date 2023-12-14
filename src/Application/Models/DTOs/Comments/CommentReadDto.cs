using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Application.Models.DTOs.Users;
using stackblob.Domain.Entities;

namespace stackblob.Application.Models.DTOs.Comments;
public class CommentReadDto : CommentBaseDto, IMapFrom<Comment>
{
    public int CommentId { get; set; }
    public int QuestionId { get; set; }
    public int? AnswerId { get; set; }
    public UserShallowReadDto? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile p)
    {
        p.CreateMap<Comment, CommentReadDto>()
            .ForMember(d => d.CreatedBy, o => o.MapFrom(src => src.CreatedByInAnswer ?? src.CreatedByInQuestion))
            .ReverseMap();
    }
}
