using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.Exceptions;

namespace stackblob.Application.UseCases.Questions.Commands.RemoveAttachment;
public class RemoveQuestionAttachmentsCommand : IRequest
{
    public int QuestionId { get; set; }
    public ICollection<int> Attachments { get; set; } = null!;
}

public class RemoveQuestionAttachmentsCommandHandler : IRequestHandler<RemoveQuestionAttachmentsCommand>
{
    private readonly IStackblobDbContext _context;

    public RemoveQuestionAttachmentsCommandHandler(IStackblobDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(RemoveQuestionAttachmentsCommand request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}
