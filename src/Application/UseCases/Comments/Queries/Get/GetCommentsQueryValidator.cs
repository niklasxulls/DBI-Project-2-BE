using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Comments.Queries.Get;
public class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
{
    public GetCommentsQueryValidator()
    {
        RuleFor(c => c.QuestionId).GreaterThanOrEqualTo(1);
        RuleFor(c => c.Paging).SetValidator(new PagingValidator<GetCommentsQuery>());
    }
}
