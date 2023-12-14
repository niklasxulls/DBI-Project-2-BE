using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;

namespace stackblob.Application.UseCases.Answers.Validators;

public class AnswerExistsValidator<T> : IAsyncPropertyValidator<T, Tuple<int, int>>
{
    private readonly IStackblobDbContext _ctx;
    private readonly int? _creatorHasToBe;
    private readonly bool _required;

    public AnswerExistsValidator(IStackblobDbContext ctx, int? creatorHasToBe = null, bool required = true) : base()
    {
        _ctx = ctx;
        _creatorHasToBe = creatorHasToBe;
        _required = required;
    }
    public string Name => "Answer exists validator";

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "The answerId doesnt exist in the database or the provided user isn't the creator of this answer";
    }

    public async Task<bool> IsValidAsync(ValidationContext<T> context, Tuple<int, int> values, CancellationToken cancellation)
    {
        var (questionId, answerId) = values;
        if (questionId < 1 || answerId < 1) return !_required;

        var dbAnswer = await _ctx.Answers.Where(q => q.AnswerId == answerId && q.QuestionId == questionId).FirstOrDefaultAsync(cancellation);

        return dbAnswer != null && (_creatorHasToBe == null || dbAnswer.CreatedById == _creatorHasToBe);
    }
}