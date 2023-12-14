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
public class RemoveQuestionCommand : IRequest
{
    public int QuestionId { get; set; }
}

public class RemoveQuestionCommandHandler : IRequestHandler<RemoveQuestionCommand>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public RemoveQuestionCommandHandler(IStackblobDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(RemoveQuestionCommand request, CancellationToken cancellationToken)
    {
        Question question = await _context.Questions.FirstAsync(q => q.QuestionId == request.QuestionId, cancellationToken);

        var answers = await _context.Answers.Where(a => a.QuestionId == question.QuestionId).ToListAsync(cancellationToken);

        _context.Answers.RemoveRange(answers);
        _context.Questions.Remove(question);

        await Task.WhenAll(
            _context.SaveChangesAsync(cancellationToken)
        );

        return Unit.Value;
    }
}
