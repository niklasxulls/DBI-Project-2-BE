using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Application.Models.DTOs.Answers;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Application.Models.DTOs.Comments;
using stackblob.Application.Models.DTOs.Tags;

namespace stackblob.Application.Models.DTOs.Questions;
public class QuestionReadDto : QuestionReadShallowDto, IMapFrom<Question>
{
    public QuestionReadDto()
    {
        Comments = new List<CommentReadDto>();
        Attachments = new List<AttachmentReadDto>();
    }
    public AnswerReadDto? CorrectAnswer { get; set; }
    public ICollection<CommentReadDto> Comments { get; set; }
    public ICollection<AttachmentReadDto> Attachments { get; set; }

    public new void Mapping(Profile p)
    {
        p.CreateMap<Question, QuestionReadDto>()
           .IncludeBase<Question, QuestionReadShallowDto>();
       
    }
}
