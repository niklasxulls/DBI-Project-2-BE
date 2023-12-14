using MediatR;
using Microsoft.AspNetCore.Mvc;
using stackblob.Application.Models.DTOs.Answers;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Application.Models.DTOs.Auth;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.UseCases.Answers.Commands.AddAttachment;
using stackblob.Application.UseCases.Answers.Commands.RemoveAttachment;
using stackblob.Application.UseCases.Answers.Commands.Votes;
using stackblob.Application.UseCases.Answers.Queries.Get;
using stackblob.Application.UseCases.Auth.Commands.Register;
using stackblob.Application.UseCases.Questions.Commands.AddAttachment;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveAttachment;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using stackblob.Application.UseCases.Questions.Commands.Votes;
using stackblob.Application.UseCases.Questions.Queries.GetById;
using stackblob.Application.UseCases.Questions.Queries.Search;
using stackblob.Rest.Controllers;

namespace Rest.Controllers;

[Route("api/answer")]
public class AnswerController : ApiControllerBase
{

    [HttpGet, Route("")]
    public async Task<ActionResult<ICollection<AnswerReadDto>>> GetAnswers([FromQuery] GetAnswersQuery query)
    {
        return (await Mediator.Send(query)).ToList();
    }

    [HttpPost, Route("")]
    public async Task<ActionResult<AnswerReadDto>> AddAnswer([FromBody] AddAnswerCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpDelete, Route("{questionId:int}/{answerId:int}")]
    public async Task<IActionResult> RemoveAnswer(int questionId, int answerId)
    {
        await Mediator.Send(new RemoveAnswerCommand { QuestionId = questionId, AnswerId = answerId });
        return NoContent();
    }

    [HttpPut, Route("")]
    public async Task<ActionResult<AnswerReadDto>> UpdateAnswer([FromBody] UpdateAnswerCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpPost, Route("vote")]
    public async Task<IActionResult> VoteAnswerCommand([FromBody] VoteAnswerCommand cmd)
    {
        await Mediator.Send(cmd);
        return NoContent();
    }

    [HttpPost, Route("attachment")]
    public async Task<ActionResult<ICollection<AttachmentReadDto>>> AddAttachments([FromForm] AddAnswerAttachmentsCommand cmd)
    {
        return (await Mediator.Send(cmd)).ToList();
    }

    [HttpDelete, Route("attachments")]
    public async Task<IActionResult> RemoveAnswerAttachments([FromBody] RemoveAnswerAttachmentsCommand cmd)
    {
        await Mediator.Send(cmd);
        return NoContent();
    }

}