using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.Models;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Validators;

namespace stackblob.Application.UseCases.Users.Queries.GetById;
public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator(IStackblobDbContext ctx)
    {
        RuleFor(u => u.UserId).SetAsyncValidator(new UserExistsValidator<GetUserQuery>(ctx, required: true)!);
    }
}
