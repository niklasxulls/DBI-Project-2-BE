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

namespace stackblob.Application.UseCases.Answers.Commands.RemoveAttachment;
public class RemoveAnswerAttachmentsCommand : IRequest
{
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
    public ICollection<int> Attachments { get; set; } = null!;
}

public class RemoveAnswerAttachmentsCommandHandler : IRequestHandler<RemoveAnswerAttachmentsCommand>
{
    private readonly IStackblobDbContext _context;

    public RemoveAnswerAttachmentsCommandHandler(IStackblobDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(RemoveAnswerAttachmentsCommand request, CancellationToken cancellationToken)
    {

        return Unit.Value;
    }
}
