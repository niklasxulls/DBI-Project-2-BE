using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.UseCases.Questions.Commands.Votes;
using stackblob.Application.UseCases.Questions.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.Votes;

public class VoteQuestionCommandValidator : AbstractValidator<VoteQuestionCommand>
{
    public VoteQuestionCommandValidator(IStackblobDbContext ctx)
    {
        RuleFor(a => a.QuestionId).SetAsyncValidator(new QuestionExistsValidator<VoteQuestionCommand>(ctx, required: true));

    }
}
