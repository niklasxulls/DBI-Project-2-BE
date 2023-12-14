using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Attributes;
using stackblob.Application.Interfaces;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;

namespace stackblob.Application.UseCases.Questions.Commands.AcceptAnswer;

[Authorize]
public class AcceptAnswerCommand : IRequest
{
    public int AnswerId { get; set; }
    public int QuestionId { get; set; }
}

public class AcceptAnswerCommandHandler : IRequestHandler<AcceptAnswerCommand>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;

    public AcceptAnswerCommandHandler(IStackblobDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(AcceptAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FirstAsync(q => q.QuestionId == request.QuestionId, cancellationToken);
        question.CorrectAnswerId = request.AnswerId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
