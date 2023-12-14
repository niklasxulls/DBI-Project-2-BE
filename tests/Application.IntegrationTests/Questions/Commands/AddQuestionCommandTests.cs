using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using Xunit;

namespace stackblob.Application.IntegrationTests.Questions.Commands;

public class AddQuestionCommandTests : TestBase
{
    public AddQuestionCommandTests(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldAddQuestion()
    {
        AddQuestionCommand cmd = new AddQuestionCommand
        {
            Title = "Test Question",
            Description = "Content of my Test Question"
        };

        var questionDto = await SendMediator(cmd);

        var question = _context.Questions.Where(u => u.QuestionId == questionDto.QuestionId).FirstOrDefault();

        question.Should().NotBeNull();
        question!.CreatedById.Should().Be(DefaultUser.UserId);
        question.Title.Should().Be(cmd.Title);
        question.Description.Should().Be(cmd.Description);
    }

    [Fact]
    public async Task ShouldAddQuestionWithTags()
    {
        var tags = _context.Tags.Take(2).ToList();

        AddQuestionCommand cmd = new AddQuestionCommand
        {
            Title = "Test Question",
            Description = "Content of my Test Question",
            Tags = tags
        };

        var questionDto = await SendMediator(cmd);

        var question = _context.Questions.Where(u => u.QuestionId == questionDto.QuestionId).FirstOrDefault();

        question.Should().NotBeNull();
        question!.Tags.Count().Should().Be(tags.Count);
        question.Tags.Should().OnlyContain(c => tags.Any(t => t.TagId == c.TagId));
    }

    [Fact]
    public async Task ShouldAddQuestionWithAttachments()
    {
        var attachmentOne = new Attachment()
        {
            Name = "test.png",
            RelativePath = "/test/test.png",
            Size = 10000,
            TypeId = AttachmentType.UpcomingQuestionAttachment,
        };
        var attachmentTwo = new Attachment()
        {
            Name = "test2.png",
            RelativePath = "/test/test2.png",
            Size = 10000,
            TypeId = AttachmentType.UpcomingQuestionAttachment,
        };
        var attachments = new List<Attachment>() { attachmentOne, attachmentTwo };
        _context.Attachments.AddRange(attachments);
        _context.SaveChanges();


        AddQuestionCommand cmd = new AddQuestionCommand
        {
            Title = "Test Question",
            Description = "Content of my Test Question",
            Attachments = attachments.Select(a => a.AttachmentId).ToList()
        };

        var questionDto = await SendMediator(cmd);
        var question = _context.Questions.Where(u => u.QuestionId == questionDto.QuestionId).FirstOrDefault();

        question.Should().NotBeNull();
        question!.Attachments.Should().NotBeNull();
        question!.Attachments.Count.Should().Be(attachments.Count);
        question!.Attachments.First(a => a.AttachmentId == attachments.First().AttachmentId).Name.Should().Be(attachments.First().Name);
    }
}
