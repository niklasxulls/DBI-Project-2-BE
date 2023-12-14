using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.Attributes;
using Microsoft.EntityFrameworkCore;

namespace stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;


[Authorize]
public class RemoveAnswerCommand : IRequest
{
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
}

public class RemoveAnswerCommandHandler : IRequestHandler<RemoveAnswerCommand>
{
    private readonly IStackblobDbContext _context;

    public RemoveAnswerCommandHandler(IStackblobDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(RemoveAnswerCommand request, CancellationToken cancellationToken)
    {
        Answer answer = await _context.Answers.FirstAsync(q => q.QuestionId == request.QuestionId && q.AnswerId == request.AnswerId, cancellationToken);

        _context.Answers.Remove(answer);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
