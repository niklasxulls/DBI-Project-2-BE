using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.AddQuestion;
public class AddAnswerCommandValidator : AbstractValidator<AddAnswerCommand>
{
    public AddAnswerCommandValidator(IStackblobDbContext ctx)
    {
        RuleFor(q => q.QuestionId).NotEmpty().GreaterThanOrEqualTo(1).SetAsyncValidator(new QuestionExistsValidator<AddAnswerCommand>(ctx, required: true));

        RuleFor(q => q.Title).NotEmpty().MaximumLength(50);
        RuleFor(q => q.Description).NotEmpty().MaximumLength(10000);

        RuleFor(a => a.Attachments).MustAsync(async (attachments, cancellationToken) =>
        {
            if (attachments?.Any() ?? false)
            {
                var dbAttachments = await ctx.Attachments.Where(a => a.TypeId == AttachmentType.UpComingAnswerAttachment && attachments.Contains(a.AttachmentId)).ToListAsync(cancellationToken);
                return dbAttachments.Count == attachments.Count;
            }

            return true;
        });

    }
}
