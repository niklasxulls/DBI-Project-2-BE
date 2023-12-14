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
using stackblob.Application.Models.DTOs.Attachments;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace stackblob.Application.UseCases.Questions.Commands.AddAttachment;


[Authorize]
public class AddQuestionAttachmentsCommand : IRequest<ICollection<AttachmentReadDto>>
{
    public int? QuestionId { get; set; }
    public ICollection<IFormFile> Attachments { get; set; } = null!;
}

public class AddQuestionAttachmentsCommandHandler : IRequestHandler<AddQuestionAttachmentsCommand, ICollection<AttachmentReadDto>>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ICurrentUserService _currentUser;

    public AddQuestionAttachmentsCommandHandler(IStackblobDbContext context, IMapper mapper, IFileService fileService, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
        _currentUser = currentUser;
    }
    public async Task<ICollection<AttachmentReadDto>> Handle(AddQuestionAttachmentsCommand request, CancellationToken cancellationToken)
    {
        var type = request.QuestionId.GetValueOrDefault() < 1 ? AttachmentType.UpcomingQuestionAttachment : AttachmentType.QuestionAttachment;

        var attachments = await _fileService.UploadFilesTo(type, cancellationToken, request.Attachments.ToArray());
 
        foreach(var attachment in attachments)
        {
            attachment.QuestionId = request.QuestionId;
        }

        await _context.Attachments.AddRangeAsync(attachments, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        return _mapper.Map<List<AttachmentReadDto>>(attachments);
    }
}