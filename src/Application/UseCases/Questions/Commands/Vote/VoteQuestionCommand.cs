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
    private readonly ICurrentUserService _currentUser;

    public VoteQuestionCommandHandler(IStackblobDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    public async Task<Unit> Handle(VoteQuestionCommand request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}

