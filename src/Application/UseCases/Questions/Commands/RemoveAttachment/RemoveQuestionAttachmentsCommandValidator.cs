using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Models;
using stackblob.Application.UseCases.Questions.Commands.AddAttachment;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.RemoveAttachment;
public class RemoveQuestionAttachmentsCommandValidator : AbstractValidator<RemoveQuestionAttachmentsCommand>
{
    public RemoveQuestionAttachmentsCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(a => a.QuestionId).SetAsyncValidator(new QuestionExistsValidator<RemoveQuestionAttachmentsCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true)!);

        RuleFor(a => a).MustAsync(async (cmd, cancellationToken) =>
        {
            var attachments = cmd.Attachments;

            if (attachments?.Any() ?? false)
            {
                var dbAttachments = await ctx.Attachments.Where(a => a.TypeId == AttachmentType.QuestionAttachment 
                                                                    && attachments.Contains(a.AttachmentId)
                                                                    && a.QuestionId == cmd.QuestionId
                                                          )
                                                         .ToListAsync(cancellationToken);
                return dbAttachments.Count >= attachments.Count;
            }

            return false;
        });
    }
}
