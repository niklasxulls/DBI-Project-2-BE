using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Application.Models.DTOs.Answers;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Application.Models.DTOs.Tags;

namespace stackblob.Application.Models.DTOs.Questions;
public class QuestionReadShallowDto : QuestionBaseDto, IMapFrom<Question>
{
    public QuestionReadShallowDto()
     {
        Tags = new List<TagReadDto>();
    }
    public int QuestionId { get; set; }
    public Guid QuestionIdAccess { get; set; }
    public int VoteCnt { get; set; }
    public ICollection<TagReadDto> Tags { get; set; }

    public void Mapping(Profile p)
    {
    }

}
