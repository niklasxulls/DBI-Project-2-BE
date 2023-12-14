using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;
using FluentValidation;
using stackblob.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace stackblob.Application.UseCases.Questions.Validators;
public class QuestionExistsValidator<T> : IAsyncPropertyValidator<T, int>
{
    private readonly IStackblobDbContext _ctx;
    private readonly int? _creatorHasToBe;
    private readonly bool _required;

    public QuestionExistsValidator(IStackblobDbContext ctx, int? creatorHasToBe = null, bool required = true) : base()
    {
        _ctx = ctx;
        _creatorHasToBe = creatorHasToBe;
        _required = required;
    }
    public string Name => "Question exists validator";

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "The questionId doesnt exist in the database or the provided user isn't the creator of this question";
    }

    public async Task<bool> IsValidAsync(ValidationContext<T> context, int value, CancellationToken cancellation)
    {
        if (value < 1) return !_required;

        var dbQuestion = await _ctx.Questions.Where(q => q.QuestionId == value).FirstOrDefaultAsync(cancellation);

        return  dbQuestion != null && (_creatorHasToBe == null || dbQuestion.CreatedById == _creatorHasToBe);
    }
}
