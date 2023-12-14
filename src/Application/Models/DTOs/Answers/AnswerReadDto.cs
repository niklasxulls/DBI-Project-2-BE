using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.Models.DTOs.Users;

namespace stackblob.Application.Models.DTOs.Answers;
public class AnswerReadDto : AnswerBaseDto, IMapFrom<Answer>
{
    public AnswerReadDto()
    {
    }
    public int AnswerId { get; set; }
    public int VoteCnt { get; set; }
    public UserShallowReadDto CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile p)
    {
    }
}
