using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Answers.Validators;
using stackblob.Application.UseCases.Questions.Commands.AddAttachment;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Answers.Commands.AddAttachment;
public class AddAnswerAttachmentsValidator : AbstractValidator<AddAnswerAttachmentsCommand>
{
    public AddAnswerAttachmentsValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(a => a.Attachments).SetValidator(new IFormFileCollectionValidator<AddAnswerAttachmentsCommand>(required: true)!);
        RuleFor(a => a.QuestionId).SetAsyncValidator(new QuestionExistsValidator<AddAnswerAttachmentsCommand>(ctx, required: true)!).DependentRules(() =>
        {

            RuleFor(a => a).MustAsync(async (cmd, cancellationToken) =>
            {
                if (cmd.AnswerId.GetValueOrDefault() < 1) return true;

                var validator = new AnswerExistsValidator<AddAnswerAttachmentsCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true);

                return await validator.IsValidAsync(new ValidationContext<AddAnswerAttachmentsCommand>(cmd), Tuple.Create(cmd.QuestionId, cmd.AnswerId ?? 0), cancellationToken);
            });
        });
    }
}
