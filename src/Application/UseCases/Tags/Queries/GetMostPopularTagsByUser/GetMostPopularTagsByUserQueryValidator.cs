using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Tags.Queries.GetMostPopularTagsByUser;
public class GetMostPopularTagsByUserQueryValidator : AbstractValidator<GetMostPopularTagsByUserQuery>
{
    public GetMostPopularTagsByUserQueryValidator()
    {
        RuleFor(q => q.UserId).GreaterThan(0);
        RuleFor(q => q.Paging).SetValidator(new PagingValidator<GetMostPopularTagsByUserQuery>(required: true));
    }
}
