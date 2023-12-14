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
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using Xunit;


namespace stackblob.Application.IntegrationTests.Questions.Commands;

public class AddQuestionAttachmentTests : TestBase
{
    private IFormFile _file;
    public AddQuestionAttachmentTests(SetupFixture setup) : base(setup)
    {
        var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        _file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/html"
        };
    }


    [Fact]
    public async Task ShouldAddQuestionAttachment()
    {
        var question = _context.Questions.First(u => u.CreatedById == DefaultUser.UserId);

        AddQuestionAttachmentsCommand cmd = new AddQuestionAttachmentsCommand()
        {
            QuestionId = question.QuestionId,
            Attachments = new List<IFormFile>() { _file }
        };

        await SendMediator(cmd);

        var newQestion = _context.Questions.Where(u => u.QuestionId == question.QuestionId).First();

        newQestion.Attachments.Should().HaveCount(cmd.Attachments.Count);
        
        foreach(var attachment in newQestion.Attachments)
        {
            (await _fileService.AttachmentExists(attachment, default)).Should().BeTrue();
        }


    }

    [Fact]
    public async Task ShouldFailAuthDuringAddQuestionAttachment()
    {
        var question = _context.Questions.First(u => u.CreatedById != DefaultUser.UserId);

        AddQuestionAttachmentsCommand cmd = new AddQuestionAttachmentsCommand
        {
            QuestionId = question.QuestionId,
            Attachments = new List<IFormFile>() { _file }
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
