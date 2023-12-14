using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.AddAttachment;
public class AddQuestionAttachmentsValidator : AbstractValidator<AddQuestionAttachmentsCommand>
{
    public AddQuestionAttachmentsValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(a => a.Attachments) .SetValidator(new IFormFileCollectionValidator<AddQuestionAttachmentsCommand>(required: true)!).DependentRules(() =>
        {

            RuleFor(a => a).MustAsync(async (cmd, cancellationToken) =>
            {
                if (cmd.QuestionId.GetValueOrDefault() < 1) return true;

                var validator = new QuestionExistsValidator<AddQuestionAttachmentsCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true);

                return await validator.IsValidAsync(new ValidationContext<AddQuestionAttachmentsCommand>(cmd), cmd.QuestionId ?? 0, cancellationToken);


            });

        });
    }
}
