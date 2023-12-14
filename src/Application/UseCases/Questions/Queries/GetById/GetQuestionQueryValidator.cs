using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;

namespace stackblob.Application.UseCases.Questions.Queries.GetById;
public class GetQuestionQueryValidator : AbstractValidator<GetQuestionQuery>
{
    public GetQuestionQueryValidator(IStackblobDbContext ctx)
    {
        RuleFor(q => q.QuestionIdAccess).NotEmpty().DependentRules(() =>
        {
            RuleFor(q => q).NotEmpty().MustAsync(async (question, cancellationToken) =>
            {
                var dbQuestion = await ctx.Questions.FirstOrDefaultAsync(q => q.QuestionIdAccess == question.QuestionIdAccess);

                return dbQuestion != null;
            });
        });
    }
}
