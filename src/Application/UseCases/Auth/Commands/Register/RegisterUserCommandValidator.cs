using stackblob.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.UseCases.Auth.Commands.Register
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(IStackblobDbContext context)
        {
            RuleFor(r => r.Email).NotEmpty().MaximumLength(50).MustAsync(async (email, _) =>
            {
                if (!email.Contains("@")) return false;
                return await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync() == null;

            });
            RuleFor(r => r.Lastname).NotEmpty().MaximumLength(50);
            RuleFor(r => r.Firstname).NotEmpty().MaximumLength(50);
            RuleFor(r => r.Password).NotEmpty().MaximumLength(64);


        }
    }
}
