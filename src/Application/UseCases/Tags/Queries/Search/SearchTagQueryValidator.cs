using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Tags.Queries.Search;
public class SearchTagQueryValidator : AbstractValidator<SearchTagQuery>
{
    public SearchTagQueryValidator()
    {
        RuleFor(t => t.SearchTerm).NotEmpty();
        RuleFor(q => q.Paging).SetValidator(new PagingValidator<SearchTagQuery>(required: true));
    }
}
