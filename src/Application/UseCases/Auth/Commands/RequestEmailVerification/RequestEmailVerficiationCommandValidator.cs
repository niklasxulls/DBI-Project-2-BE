using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;

namespace stackblob.Application.UseCases.Auth.Commands.RequestEmailVerification;
public class RequestEmailVerficiationCommandValidator : AbstractValidator<RequestEmailVerificationCommand>
{
    public RequestEmailVerficiationCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(r => r).MustAsync(async (request, cancellationToken) =>
        {
            var verifiedVerifications = await ctx.UserEmailVerifications.Where(u => u.UserId == currentUser.UserId).FirstOrDefaultAsync(cancellationToken);

            if (verifiedVerifications != null)
                throw new Exception("User email already verified!");

            return true;
        });
    }
}
