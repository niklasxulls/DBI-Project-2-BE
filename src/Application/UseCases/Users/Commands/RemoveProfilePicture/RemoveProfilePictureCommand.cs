using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Attributes;

namespace stackblob.Application.UseCases.Users.Commands.RemoveProfilePicture;

[Authorize]
public class RemoveProfilePictureCommand : IRequest<Task>
{
}

public class RemoveProfilePictureCommandHandler : IRequestHandler<RemoveProfilePictureCommand, Task>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IStackblobDbContext _context;
    private readonly IFileService _fileService;

    public RemoveProfilePictureCommandHandler(ICurrentUserService currentUser, IStackblobDbContext context, IFileService fileService)
    {
        _currentUser = currentUser;
        _context = context;
        _fileService = fileService;
    }
    public async Task<Task> Handle(RemoveProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(u => u.ProfilePicture).FirstAsync(u => u.UserId == _currentUser.UserId, cancellationToken);

        await _fileService.RemoveAttachments(cancellationToken, user.ProfilePicture!);
        user.ProfilePicture = null;


        return await Task.FromResult(Task.CompletedTask);
    }
}
