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
    private readonly IFileService _fileService;

    public RemoveQuestionAttachmentsCommandHandler(IStackblobDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }
    public async Task<Unit> Handle(RemoveQuestionAttachmentsCommand request, CancellationToken cancellationToken)
    {
        List<Attachment> dbAttachments = await _context.Attachments.Where(a => request.Attachments.Contains(a.AttachmentId)).ToListAsync(cancellationToken);

        var removedAttachments = await _fileService.RemoveAttachments(cancellationToken, dbAttachments.ToArray());
        
        var failedToRemoveAttachments = new List<Attachment>();
        var successfullyRemovedAttachments = new List<Attachment>();

        dbAttachments.ForEach(a =>
        {
            if (!removedAttachments.Any(ra => a.AttachmentId == ra))
            {
                failedToRemoveAttachments.Add(a);
            } else
            {
                successfullyRemovedAttachments.Add(a);
            }
        });

        _context.Attachments.RemoveRange(successfullyRemovedAttachments);
        await _context.SaveChangesAsync(cancellationToken);

        if(removedAttachments.Count != dbAttachments.Count)
        {
            throw new ActionOnlyPartlyFullfiledException($"Couldn't delete attachments {failedToRemoveAttachments.Select(a => a.AttachmentId)}");
        }

        return Unit.Value;
    }
}
