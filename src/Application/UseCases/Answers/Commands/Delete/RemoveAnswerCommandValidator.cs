using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Answers.Validators;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
public class RemoveAnswerCommandValidator : AbstractValidator<RemoveAnswerCommand>
{
    public RemoveAnswerCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(a => Tuple.Create(a.QuestionId, a.AnswerId)).SetAsyncValidator(new AnswerExistsValidator<RemoveAnswerCommand>(ctx, creatorHasToBe: currentUser.UserId,  required: true)!);
    }
}
