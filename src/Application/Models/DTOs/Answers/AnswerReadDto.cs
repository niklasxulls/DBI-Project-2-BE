using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Application.Models.DTOs.Comments;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.Models.DTOs.Users;

namespace stackblob.Application.Models.DTOs.Answers;
public class AnswerReadDto : AnswerBaseDto, IMapFrom<Answer>
{
    public AnswerReadDto()
    {
        Comments = new List<CommentReadDto>();
        Attachments = new List<AttachmentReadDto>();
    }
    public int AnswerId { get; set; }
    public int VoteCnt { get; set; }
    public ICollection<CommentReadDto> Comments { get; set; }
    public ICollection<AttachmentReadDto> Attachments { get; set; }
    public UserShallowReadDto CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile p)
    {
        p.CreateMap<Answer, AnswerReadDto>()
           .ForMember(d => d.VoteCnt, o => o.MapFrom(src => src.AnswerVotes.Sum(v => v.IsUpVote ? 1 : -1)));
    }
}
