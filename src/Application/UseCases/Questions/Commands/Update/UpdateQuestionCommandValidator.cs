using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using stackblob.Application.UseCases.Questions.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.Update;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(a => a.QuestionId).GreaterThanOrEqualTo(1).SetAsyncValidator(new QuestionExistsValidator<UpdateQuestionCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true));
        RuleFor(q => q.Description).NotEmpty().MaximumLength(10000);
        RuleFor(q => q.Title).NotEmpty().MaximumLength(150);
    }
}
