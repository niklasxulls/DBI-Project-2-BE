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
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;

namespace stackblob.Application.UseCases.Questions.Commands.AddQuestion;

[Authorize]
public class UpdateAnswerCommand : AnswerWriteDto, IRequest<AnswerReadDto>, IMapFrom<Answer>
{
    public int AnswerId { get; set; }
    public new void Mapping(Profile p)
    {
        p.CreateMap<UpdateAnswerCommand, Answer>()
         .IncludeBase<AnswerWriteDto, Answer>();
         
    }

}

public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, AnswerReadDto>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAnswerCommandHandler(IStackblobDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<AnswerReadDto> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        Answer answer = _mapper.Map<Answer>(request);

        Answer currentAnswer = (await _context.Answers.FindAsync(request.AnswerId))!;
        _context.Entry(currentAnswer).CurrentValues.SetValues(answer);

        await _context.SaveChangesAsync(cancellationToken);


        return _mapper.Map<AnswerReadDto>(answer);
    }
}
