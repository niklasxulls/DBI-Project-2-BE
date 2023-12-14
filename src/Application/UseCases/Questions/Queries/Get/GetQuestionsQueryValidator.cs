using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Queries.Search;

public class GetQuestionsQueryValidator : AbstractValidator<GetQuestionsQuery>
{
    public GetQuestionsQueryValidator()
    {
        RuleFor(q => q.Paging).SetValidator(new PagingValidator<GetQuestionsQuery>(required: true));
    }
}
