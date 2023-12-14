using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
public class RemoveQuestionCommandValidator : AbstractValidator<RemoveQuestionCommand>
{

    public RemoveQuestionCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(a => a.QuestionId).SetAsyncValidator(new QuestionExistsValidator<RemoveQuestionCommand>(ctx, creatorHasToBe: currentUser.UserId,  required: true)!);
    }
}
