using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;

namespace stackblob.Application.UseCases.Users.Commands.Remove;

[Authorize]
public class RemoveUserCommand : IRequest
{
    
}

public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand>
{
    private readonly IStackblobDbContext _ctx;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileService _fileService;

    public RemoveUserCommandHandler(IStackblobDbContext ctx, ICurrentUserService currentUser, IFileService fileService)
    {
        _ctx = ctx;
        _currentUser = currentUser;
        _fileService = fileService;
    }
    public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _ctx.Users.Include(u => u.ProfilePicture).Include(u => u.Banner).FirstAsync(u => u.UserId == _currentUser.UserId, cancellationToken);

        if(user.ProfilePicture != null)
        {
            await _fileService.RemoveAttachments(cancellationToken, user.ProfilePicture);
            _ctx.Attachments.Remove(user.ProfilePicture);
        }
        if (user.Banner != null)
        {
            await _fileService.RemoveAttachments(cancellationToken, user.Banner);
            _ctx.Attachments.Remove(user.Banner);
        }

        _ctx.Users.Remove(user);
        await _ctx.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
