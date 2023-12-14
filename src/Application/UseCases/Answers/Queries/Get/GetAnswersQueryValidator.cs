using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.Interfaces;
using stackblob.Application.UseCases.Questions.Validators;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Answers.Queries.Get;

public class GetAnswersQueryValidator : AbstractValidator<GetAnswersQuery>
{
    public GetAnswersQueryValidator(IStackblobDbContext ctx)
    {
        RuleFor(q => q.QuestionId).GreaterThanOrEqualTo(1).SetAsyncValidator(new QuestionExistsValidator<GetAnswersQuery>(ctx, required: true));
        RuleFor(q => q.Paging).SetValidator(new PagingValidator<GetAnswersQuery>(required: true));
    }

}
