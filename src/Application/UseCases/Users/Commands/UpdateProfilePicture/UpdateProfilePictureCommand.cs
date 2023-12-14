using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Models.DTOs.Attachments;

namespace stackblob.Application.UseCases.Users.Commands.UpdateProfilePicture;

[Authorize]
public class UpdateProfilePictureCommand : IRequest<string>
{
    public IFormFile ProfilePicture { get; set; } = null!;
}

public class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand, string>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IFileService _fileService;

    public UpdateProfilePictureCommandHandler(ICurrentUserService currentUser, IStackblobDbContext context, IMapper mapper,
        IFileService fileService)
    {
        _currentUser = currentUser;
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }
    public async Task<string> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(u => u.ProfilePicture).FirstAsync(u => u.UserId == _currentUser.UserId);

        if (user.ProfilePicture != null)
        {
            await _fileService.RemoveAttachments(cancellationToken, user.ProfilePicture);
        }

        var newProfilePicture = await _fileService.UploadFilesTo(AttachmentType.ProfileAvatar, cancellationToken, request.ProfilePicture);

        user.ProfilePicture = newProfilePicture.First();

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AttachmentReadDto>(user.ProfilePicture)?.Url ?? "";

    }
}
