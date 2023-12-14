using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Answers.Validators;
using stackblob.Application.UseCases.Comments.Validators;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Comments.Commands.RemoveQuestion;
public class RemoveCommentCommandValidator : AbstractValidator<RemoveCommentCommand>
{
    public RemoveCommentCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(a => a.CommentId).SetAsyncValidator(new CommentExistsValidator<RemoveCommentCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true)!);
    }
}
