using stackblob.Application.Models;
using stackblob.Application.UseCases.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stackblob.Application.UseCases.Auth.Queries;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Commands.AddAttachment;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Application.UseCases.Questions.Commands.RemoveAttachment;
using stackblob.Application.UseCases.Questions.Queries.Search;
using stackblob.Application.UseCases.Questions.Queries.GetById;
using stackblob.Application.UseCases.Questions.Commands.Votes;
using stackblob.Application.UseCases.Questions.Commands.AcceptAnswer;
using stackblob.Application.UseCases.Questions.Commands.RevertAcceptAnswer;

namespace stackblob.Rest.Controllers;

[Route("api/question")]
public class QuestionController : ApiControllerBase
{
    [HttpGet, Route("{questionId:Guid}")]
    public async Task<ActionResult<QuestionReadDto>> GetQuestion(Guid questionIdAccess)
    {
        return await Mediator.Send(new GetQuestionQuery { QuestionIdAccess = questionIdAccess });
    }

    [HttpGet, Route("")]
    public async Task<ActionResult<ICollection<QuestionReadShallowDto>>> GetQuestions([FromQuery] GetQuestionsQuery query)
    {
        return (await Mediator.Send(query)).ToList();
    }

    [HttpPost, Route("")]
    public async Task<ActionResult<QuestionReadDto>> AddQuestion([FromBody] AddQuestionCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpDelete, Route("{questionId:int}")]
    public async Task<IActionResult> RemoveQuestion(int questionId)
    {
        await Mediator.Send(new RemoveAnswerCommand { QuestionId = questionId });
        return NoContent();
    }

    [HttpPut, Route("")]
    public async Task<ActionResult<QuestionReadDto>> UpdateQuestion([FromBody] UpdateQuestionCommand cmd)
    {
        return await Mediator.Send(cmd);
    }


    [HttpPost, Route("attachment")]
    public async Task<ActionResult<ICollection<AttachmentReadDto>>> AddAttachments([FromForm] AddQuestionAttachmentsCommand cmd)
    {
        return (await Mediator.Send(cmd)).ToList();
    }

    [HttpDelete, Route("attachments")]
    public async Task<IActionResult> RemoveQuestionAttachments([FromBody] RemoveQuestionAttachmentsCommand cmd)
    {
        await Mediator.Send(cmd);
        return NoContent();
    }

    [HttpPost, Route("vote")]
    public async Task<IActionResult> VoteQuestionCommand([FromBody] VoteQuestionCommand cmd)
    {
        await Mediator.Send(cmd);
        return NoContent();
    }

    [HttpPost, Route("accept-answer")]
    public async Task<IActionResult> AcceptAnswerCommand([FromBody] AcceptAnswerCommand cmd)
    {
        await Mediator.Send(cmd);
        return NoContent();
    }

    [HttpPost, Route("revert-accept-answer")]
    public async Task<IActionResult> AcceptAnswerCommand([FromBody] RevertAcceptAnswerCommand cmd)
    {
        await Mediator.Send(cmd);
        return NoContent();
    }




}
