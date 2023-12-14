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

namespace stackblob.Application.UseCases.Questions.Commands.Votes;


[Authorize]
public class VoteQuestionCommand : IRequest
{
    public int QuestionId { get; set; }
    public bool IsUpVote { get; set; }
}

public class VoteQuestionCommandHandler : IRequestHandler<VoteQuestionCommand>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ICurrentUserService _currentUser;

    public VoteQuestionCommandHandler(IStackblobDbContext context, IMapper mapper, IFileService fileService, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
        _currentUser = currentUser;
    }
    public async Task<Unit> Handle(VoteQuestionCommand request, CancellationToken cancellationToken)
    {
        var vote = await _context.Votes.FirstOrDefaultAsync(q => q.CreateByInQuestionId == _currentUser.UserId && q.QuestionId == request.QuestionId, cancellationToken);

        if(vote != null)
        {
            vote.IsUpVote = request.IsUpVote;
        } else
        {
            await _context.Votes.AddAsync(new Vote()
            {
                QuestionId = request.QuestionId,
                CreateByInQuestionId = _currentUser.UserId ?? 0,
                IsUpVote = request.IsUpVote,
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

