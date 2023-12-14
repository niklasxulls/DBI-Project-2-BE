using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces;

namespace stackblob.Application.UseCases.Auth.Commands.Verify;
public class VerifyUserEmailCommandValidator : AbstractValidator<VerifyUserEmailCommand>
{
    public VerifyUserEmailCommandValidator(IStackblobDbContext ctx)
    {
        RuleFor(l => l.Guid).NotEmpty().MustAsync(async (verificationGuid, cancellationToken) =>
        {
            var verification = await ctx.UserEmailVerifications.FirstOrDefaultAsync(u => u.UserEmailVerificationAccess == verificationGuid, cancellationToken);

            if (verification == null)
                throw new NotFoundException("Verification not found!");

            if (verification.ExpiresAt < DateTimeUtil.Now())
                throw new Exception("Verification already expired!");


            return true;
        });
    }
}