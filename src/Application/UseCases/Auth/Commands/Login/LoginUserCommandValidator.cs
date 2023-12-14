using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.UseCases.Auth.Commands.Login
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator(IStackblobDbContext context, IAuthService auth)
        {
            RuleFor(l => l.Email).NotEmpty().MaximumLength(50);
            RuleFor(l => l.Password).NotEmpty().MaximumLength(100).DependentRules(() =>
            {
                RuleFor(l => l).MustAsync(async (loginCmd, cancellationToken) =>
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(loginCmd.Email.ToLower()), cancellationToken);

                    if (user == null || !auth.PasswordsEqual(plainPassword: loginCmd.Password, salt: user.Salt, hashedPassword: user.Password))
                        throw new NotFoundException("Email or password wrong!");

                    return true;
                });
            });



        }
    }
}
