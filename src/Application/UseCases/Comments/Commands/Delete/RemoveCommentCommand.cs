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

namespace stackblob.Application.UseCases.Comments.Commands.RemoveQuestion;


[Authorize]
public class RemoveCommentCommand : IRequest
{
    public int CommentId { get; set; }
}

public class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand>
{
    private readonly IStackblobDbContext _context;

    public RemoveCommentCommandHandler(IStackblobDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        Comment comment = await _context.Comments.FirstAsync(c => c.CommentId == request.CommentId, cancellationToken);

        _context.Comments.Remove(comment);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
