using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.UseCases.Questions.Queries.GetQuestionTags;
public class GetQuestionUsersQueryValidator : AbstractValidator<GetQuestionTagsQuery>
{
    public GetQuestionUsersQueryValidator()
    {
    }
}
