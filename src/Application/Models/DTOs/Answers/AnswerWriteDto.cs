using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using stackblob.Application.Mapping;

namespace stackblob.Application.Models.DTOs.Answers;
public class AnswerWriteDto : AnswerBaseDto, IMapFrom<Answer>
{
  public void Mapping(Profile p)
    {
        p.CreateMap<AnswerWriteDto, Answer>()
         .ReverseMap();
    }
}
