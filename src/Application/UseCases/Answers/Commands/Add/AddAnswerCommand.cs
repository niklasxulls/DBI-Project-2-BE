using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Mapping;
using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Answers;
using stackblob.Application.Models.DTOs.Questions;

namespace stackblob.Application.UseCases.Questions.Commands.AddQuestion;

[Authorize]
public class AddAnswerCommand : AnswerWriteDto, IRequest<AnswerReadDto>, IMapFrom<Answer>
{
    public ICollection<int>? Attachments { get; set; }
    public new void Mapping(Profile p)
    {
        p.CreateMap<AddAnswerCommand, Answer>()
         .IncludeBase<AnswerWriteDto, Answer>();
    }
}

public class AddAnswerCommandHandler : IRequestHandler<AddAnswerCommand, AnswerReadDto>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;

    public AddAnswerCommandHandler(IStackblobDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<AnswerReadDto> Handle(AddAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = _mapper.Map<Answer>(request);

        await _context.Answers.AddAsync(answer, cancellationToken);

        if (request.Attachments?.Any() ?? false)
        {
        }

        await _context.SaveChangesAsync(cancellationToken);


        return _mapper.Map<AnswerReadDto>(answer);
    }
}
