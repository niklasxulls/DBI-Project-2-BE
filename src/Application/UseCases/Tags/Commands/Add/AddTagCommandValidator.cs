using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.Attributes;
using stackblob.Application.Models.DTOs.Tags;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace stackblob.Application.UseCases.Tags.Commands.Add;

public class AddTagCommandValidator : AbstractValidator<AddTagCommand>
{
    public AddTagCommandValidator(IStackblobDbContext ctx)
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Name).MustAsync(async (tagName, cancellationToken) =>
        {
            var processedTagName = StringUtil.ReplaceSpecialCharacters(tagName);

            var dbTag = await ctx.Tags.Where(t => t.Name == processedTagName).FirstOrDefaultAsync(cancellationToken);
            
            return dbTag != null;
        });
    }
}