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
public class UserExistsValidator<T> : IAsyncPropertyValidator<T, int>
{
    private readonly IStackblobDbContext _ctx;
    private readonly bool _required;

    public UserExistsValidator(IStackblobDbContext ctx,  bool required = true) : base()
    {
        _ctx = ctx;
        _required = required;
    }
    public string Name => "User exists validator";

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "The user doesnt exist in the database";
    }

    public async Task<bool> IsValidAsync(ValidationContext<T> context, int value, CancellationToken cancellation)
    {
        if (value < 1) return !_required;

        var dbQuestion = await _ctx.Users.Where(q => q.UserId == value).FirstOrDefaultAsync(cancellation);

        return  dbQuestion != null;
    }
}
