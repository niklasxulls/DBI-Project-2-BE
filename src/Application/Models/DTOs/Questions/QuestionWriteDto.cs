using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using stackblob.Application.Mapping;

namespace stackblob.Application.Models.DTOs.Questions;
public class QuestionWriteDto : QuestionBaseDto, IMapFrom<Question>
{
    public ICollection<Tag>? Tags { get; set; }

    public void Mapping(Profile p) {
    }

}
