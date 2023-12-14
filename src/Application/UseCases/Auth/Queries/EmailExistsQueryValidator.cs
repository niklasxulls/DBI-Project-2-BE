using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace stackblob.Application.UseCases.Auth.Queries;

public class EmailExistsQueryValidator : AbstractValidator<EmailExistsQuery>
{
    public EmailExistsQueryValidator()
    {
        RuleFor(e => e.SearchTerm).MaximumLength(100).NotEmpty();
        RuleFor(e => e.SearchTerm).Must(email =>
        {
            if (!email.Contains("@")) return false;
            return true;
        });
    }
}
