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

namespace stackblob.Application.UseCases.Questions.Commands.AcceptAnswer;
public class AcceptAnswerCommandValidator : AbstractValidator<AcceptAnswerCommand>
{
    public AcceptAnswerCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(c => c.QuestionId)
            .GreaterThanOrEqualTo(1).SetAsyncValidator(new QuestionExistsValidator<AcceptAnswerCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true))
            .DependentRules(() =>
            {
                RuleFor(c => c).MustAsync(async (cmd, cancellationToken) =>
                {
                    var question = await ctx.Questions.FirstOrDefaultAsync(q => q.QuestionId == cmd.QuestionId, cancellationToken);
                    if (question != null && question.CorrectAnswerId != null) return false;

                    var answer = await ctx.Answers.FirstOrDefaultAsync(a => a.QuestionId == cmd.QuestionId && a.AnswerId == cmd.AnswerId, cancellationToken);
                    return answer != null;
                });
            });
    }
}
