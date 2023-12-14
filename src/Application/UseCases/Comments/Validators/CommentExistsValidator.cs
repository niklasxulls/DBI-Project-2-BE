using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;

namespace stackblob.Application.UseCases.Comments.Validators;

public class CommentExistsValidator<T> : IAsyncPropertyValidator<T, int>
{
    private readonly IStackblobDbContext _ctx;
    private readonly int? _creatorHasToBe;
    private readonly bool _required;

    public CommentExistsValidator(IStackblobDbContext ctx, int? creatorHasToBe = null, bool required = true) : base()
    {
        _ctx = ctx;
        _creatorHasToBe = creatorHasToBe;
        _required = required;
    }
    public string Name => "Comment exists validator";

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "The commentId doesnt exist in the database or the provided user isn't the creator of this comment";
    }

    public async Task<bool> IsValidAsync(ValidationContext<T> context, int commentId, CancellationToken cancellation)
    {
        if (commentId < 1) return !_required;

        var dbComment = await _ctx.Comments.Where(c => c.CommentId == commentId).FirstOrDefaultAsync(cancellation);

        return dbComment != null && (_creatorHasToBe == null || (dbComment.CreatedByInAnswerId ?? dbComment.CreatedByInQuestionId) == _creatorHasToBe);
    }
}