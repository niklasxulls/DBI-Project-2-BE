using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.AddAttachment;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveAttachment;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using Xunit;


namespace stackblob.Application.IntegrationTests.Questions.Commands;

public class RemoveQuestionAttachmentCommandTests : TestBase
{
    private readonly IFormFile _file;
    public RemoveQuestionAttachmentCommandTests(SetupFixture setup) : base(setup)
    {
        var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        _file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/html"
        };

    }


    [Fact]
    public async Task ShouldRemoveQuestionAttachment()
    {
        var question = _context.Questions.First(u => u.CreatedById == DefaultUser.UserId);

        var addAttachmentCmd = new AddQuestionAttachmentsCommand()
        {
            Attachments = new List<IFormFile>() { _file },
            QuestionId = question.QuestionId
        };


        await SendMediator(addAttachmentCmd);
        var attachments = _context.Questions.First(q => q.QuestionId == question.QuestionId).Attachments;
        
        attachments.Should().HaveCount(addAttachmentCmd.Attachments.Count);


        RemoveQuestionAttachmentsCommand removeAttachmentCommand = new RemoveQuestionAttachmentsCommand
        {
            QuestionId = question.QuestionId,
            Attachments = attachments.Select(x => x.AttachmentId).ToList()
        };

        await SendMediator(removeAttachmentCommand);

        var updatedQuestion = _context.Questions.First(q => q.QuestionId == question.QuestionId);

        updatedQuestion.Attachments.Should().HaveCount(0);

        foreach(var attachment in attachments)
        {
            (await _fileService.AttachmentExists(attachment, default)).Should().BeFalse();
        }
    }

    [Fact]
    public async Task ShouldFailAttachmenAuth()
    {
        var question = _context.Questions.First(u => u.CreatedById != DefaultUser.UserId);

        RemoveQuestionAttachmentsCommand cmd = new RemoveQuestionAttachmentsCommand
        {
            QuestionId = question.QuestionId,
            Attachments = new List<int> { short.MaxValue }
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShoudFailAttachmentDoesntExist()
    {
        var question = _context.Questions.First(u => u.CreatedById == DefaultUser.UserId);

        RemoveQuestionAttachmentsCommand cmd = new RemoveQuestionAttachmentsCommand
        {
            QuestionId = question.QuestionId,
            Attachments = new List<int> { short.MaxValue }
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldFailAttachmentWrongType()
    {
        var question = _context.Questions.First(u => u.CreatedById == DefaultUser.UserId);

        var attachment = new Attachment()
        {
            Name = "test.txt",
            Question = question,
            TypeId = AttachmentType.AnswerAttachment,
            Size = 10000,
            RelativePath = "test.txt"
        };

        question.Attachments.Add(attachment);
        _context.SaveChanges();

        RemoveQuestionAttachmentsCommand cmd = new RemoveQuestionAttachmentsCommand
        {
            QuestionId = question.QuestionId,
            Attachments = question.Attachments.Select(q => q.AttachmentId).ToList()
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
