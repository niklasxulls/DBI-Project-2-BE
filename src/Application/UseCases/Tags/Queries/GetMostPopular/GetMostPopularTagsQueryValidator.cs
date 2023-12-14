using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Tags.Queries.GetMostPopular;
public class GetMostPopularTagsQueryValidator : AbstractValidator<GetMostPopularTagsQuery>
{
    public GetMostPopularTagsQueryValidator()
    {
        RuleFor(q => q.Start).SetValidator(new DateOnlyValidator<GetMostPopularTagsQuery>(required: true));
        RuleFor(q => q.End).SetValidator(new DateOnlyValidator<GetMostPopularTagsQuery>(required: true));
        RuleFor(q => q.Paging).SetValidator(new PagingValidator<GetMostPopularTagsQuery>(required: true));
    }
}
