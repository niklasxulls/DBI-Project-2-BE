using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.UseCases.Answers.Validators;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Comments.Commands.AddQuestion;
public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator(IStackblobDbContext ctx)
    {
        RuleFor(q => q.QuestionId).GreaterThanOrEqualTo(1).SetAsyncValidator(new QuestionExistsValidator<AddCommentCommand>(ctx, required: true));
        RuleFor(a => Tuple.Create(a.QuestionId, a.AnswerId ?? 0)).SetAsyncValidator(new AnswerExistsValidator<AddCommentCommand>(ctx, required: false));
        RuleFor(q => q.Description).MaximumLength(250);
    }
}
