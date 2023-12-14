using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.UseCases.Validators;

namespace stackblob.Application.UseCases.Questions.Commands.AddQuestion;
public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
{
    public AddQuestionCommandValidator(IStackblobDbContext ctx)
    {
        RuleFor(q => q.Description).NotEmpty().MaximumLength(10000);
        RuleFor(q => q.Title).NotEmpty().MaximumLength(150);

        RuleFor(a => a.Attachments).MustAsync(async (attachments, cancellationToken) =>
        {

            return true;
        });

    }
}
