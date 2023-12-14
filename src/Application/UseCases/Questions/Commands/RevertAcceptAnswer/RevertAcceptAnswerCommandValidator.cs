using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Questions.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.RevertAcceptAnswer;
public class RevertAcceptAnswerCommandValidator : AbstractValidator<RevertAcceptAnswerCommand>
{
    public RevertAcceptAnswerCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(c => c.QuestionId).SetAsyncValidator(new QuestionExistsValidator<RevertAcceptAnswerCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true));

    }
}
