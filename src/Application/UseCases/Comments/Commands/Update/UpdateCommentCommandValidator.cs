using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Answers.Validators;
using stackblob.Application.UseCases.Comments.Commands.AddQuestion;
using stackblob.Application.UseCases.Comments.Validators;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.AddQuestion;
public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{

    public UpdateCommentCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(q => q.CommentId).GreaterThanOrEqualTo(1).SetAsyncValidator(new CommentExistsValidator<UpdateCommentCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true));
        RuleFor(q => q.Description).NotEmpty().MaximumLength(250);
    }
}
