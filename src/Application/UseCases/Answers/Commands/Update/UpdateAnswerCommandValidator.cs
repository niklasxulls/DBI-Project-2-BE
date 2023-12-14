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
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.AddQuestion;
public class UpdateAnswerCommandValidator : AbstractValidator<UpdateAnswerCommand>
{

    public UpdateAnswerCommandValidator(IStackblobDbContext ctx, ICurrentUserService currentUser)
    {
        RuleFor(q => q.QuestionId).GreaterThanOrEqualTo(1);
        RuleFor(q => q.AnswerId).GreaterThanOrEqualTo(1);
        RuleFor(q => Tuple.Create(q.QuestionId, q.AnswerId)).SetAsyncValidator(new AnswerExistsValidator<UpdateAnswerCommand>(ctx, creatorHasToBe: currentUser.UserId, required: true));


        RuleFor(q => q.Title).NotEmpty().MaximumLength(50);
        RuleFor(q => q.Description).NotEmpty().MaximumLength(10000);
    }
}
