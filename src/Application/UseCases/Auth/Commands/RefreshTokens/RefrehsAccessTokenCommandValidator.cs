using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Application.Exceptions;
using stackblob.Domain.Entities;
using MediatR;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace stackblob.Application.UseCases.Auth.Commands.RefreshTokens
{
    public class RefrehsAccessTokenCommandValidator : AbstractValidator<RefreshTokensCommand>
    {

        public RefrehsAccessTokenCommandValidator(IStackblobDbContext context, IAuthService auth)
        {
            RuleFor(r => r.AccessToken).NotEmpty();
            RuleFor(r => r.RefreshToken).NotEmpty().Length(64).DependentRules(() =>
            {
                RuleFor(r => r).MustAsync(async (cmd, cancellationToken) =>
                {
                    var userId = auth.GetUsedIdByAccessToken(cmd.AccessToken);

                    if (userId == null)
                        throw new ForbiddenAccessException("Invalid AccessToken provided");

                    var currtoken = await context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == cmd.RefreshToken && r.UserID == userId, cancellationToken);

                    if (currtoken == null)
                        throw new ForbiddenAccessException("Invalid RefreshToken provided");

                    return true;
                });
            });
        }
    }
}
