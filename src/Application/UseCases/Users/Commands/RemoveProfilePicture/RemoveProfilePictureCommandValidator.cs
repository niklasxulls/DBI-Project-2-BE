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
using FluentValidation;

namespace stackblob.Application.UseCases.Users.Commands.RemoveProfilePicture;

public class RemoveProfilePictureCommandValidator : AbstractValidator<RemoveProfilePictureCommand>
{
    public RemoveProfilePictureCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(c => c).MustAsync(async (_, cancellationToken) =>
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == currentUser.UserId && u.ProfilePicture != null, cancellationToken);

            return user != null;
        });
    }
}
