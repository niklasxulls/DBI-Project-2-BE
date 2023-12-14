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


namespace stackblob.Application.IntegrationTests.Answers.Commands;

public class AddAnswerCommandTests : TestBase
{
    private IFormFile _file;
    public AddAnswerCommandTests(SetupFixture setup) : base(setup)
    {
        var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        _file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/html"
        };
    }



    [Fact]
    public async Task ShouldAddAnswer()
    {
        var question = _context.Questions.First(q => q.CreatedById == DefaultUser.UserId);

        AddAnswerCommand cmd = new AddAnswerCommand
        {
            QuestionId = question.QuestionId,
            Title = "Test Answer",
            Description = "Content of my Test Answer",
        };

        var answerDto = await SendMediator(cmd);

        var answer = _context.Answers.Where(u => u.AnswerId == answerDto.AnswerId).FirstOrDefault();

        answer.Should().NotBeNull();

        answer!.QuestionId.Should().Be(question.QuestionId);
        answer!.CreatedById.Should().Be(DefaultUser.UserId);
        answer!.Title.Should().Be(cmd.Title);
        answer!.Description.Should().Be(cmd.Description);
    }


    [Fact]
    public async Task ShouldAddAnswerWithAttachments()
    {
        var attachmentOne = new Attachment()
        {
            Name = "test.png",
            RelativePath = "/test/test.png",
            Size = 10000,
            TypeId = AttachmentType.UpComingAnswerAttachment,
        };
        var attachmentTwo = new Attachment()
        {
            Name = "test2.png",
            RelativePath = "/test/test2.png",
            Size = 10000,
            TypeId = AttachmentType.UpComingAnswerAttachment,
        };
        var attachments = new List<Attachment>() { attachmentOne, attachmentTwo };
        _context.Attachments.AddRange(attachments);
        _context.SaveChanges();

        var question = _context.Questions.First(q => q.CreatedById == DefaultUser.UserId);


        AddAnswerCommand cmd = new AddAnswerCommand
        {
            QuestionId = question.QuestionId,
            Title = "Test Answer",
            Description = "Content of my Test Answer",
            Attachments = attachments.Select(a => a.AttachmentId).ToList()
        };

        var answerDto = await SendMediator(cmd);
        var answer = _context.Answers.Where(u => u.AnswerId == answerDto.AnswerId).FirstOrDefault();

        answer.Should().NotBeNull();
        answer!.Attachments.Should().NotBeNull();
        answer!.Attachments.Count.Should().Be(attachments.Count);

        foreach (var attachment in attachments)
        {
            var answerAttachment = answer.Attachments.FirstOrDefault(a => a.AttachmentId == attachment.AttachmentId);

            answerAttachment.Should().NotBeNull();
            answerAttachment!.Name.Should().Be(attachment.Name);
            answerAttachment!.TypeId.Should().Be(AttachmentType.AnswerAttachment);
        }
    }
}
