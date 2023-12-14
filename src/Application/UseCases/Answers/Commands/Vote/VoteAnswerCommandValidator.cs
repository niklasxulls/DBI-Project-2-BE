using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.UseCases.Answers.Validators;
using stackblob.Application.UseCases.Questions.Commands.Votes;
using stackblob.Application.UseCases.Questions.Validators;

namespace stackblob.Application.UseCases.Answers.Commands.Votes;

public class VoteAnswerCommandValidator : AbstractValidator<VoteAnswerCommand>
{
    public VoteAnswerCommandValidator(IStackblobDbContext ctx)
    {
        RuleFor(a => Tuple.Create(a.QuestionId, a.AnswerId)).SetAsyncValidator(new AnswerExistsValidator<VoteAnswerCommand>(ctx, required: true));
    }
}
