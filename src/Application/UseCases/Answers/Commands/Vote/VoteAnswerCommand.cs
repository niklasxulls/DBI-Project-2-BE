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

namespace stackblob.Application.UseCases.Answers.Commands.Votes;


[Authorize]
public class VoteAnswerCommand : IRequest
{
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
    public bool IsUpVote { get; set; }
}

public class VoteAnswerCOmmandHandler : IRequestHandler<VoteAnswerCommand>
{
    private readonly IStackblobDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public VoteAnswerCOmmandHandler(IStackblobDbContext context,  ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    public async Task<Unit> Handle(VoteAnswerCommand request, CancellationToken cancellationToken)
    {
        var vote = await _context.Votes.FirstOrDefaultAsync(q => q.CreateByInAnswerId == _currentUser.UserId && q.QuestionId == request.QuestionId && q.AnswerId == q.AnswerId, cancellationToken);

        if(vote != null)
        {
            vote.IsUpVote = request.IsUpVote;
        } else
        {
            await _context.Votes.AddAsync(new Vote()
            {
                QuestionId = request.QuestionId,
                AnswerId = request.AnswerId,
                CreateByInAnswerId = _currentUser.UserId ?? 0,
                IsUpVote = request.IsUpVote,
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

