using MediatR;
using Microsoft.AspNetCore.Mvc;
using stackblob.Application.Models.DTOs.Answers;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Application.Models.DTOs.Auth;
using stackblob.Application.Models.DTOs.Comments;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.UseCases.Answers.Commands.Votes;
using stackblob.Application.UseCases.Answers.Queries.Get;
using stackblob.Application.UseCases.Auth.Commands.Register;
using stackblob.Application.UseCases.Comments.Commands.AddQuestion;
using stackblob.Application.UseCases.Comments.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Comments.Queries.Get;
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

[Route("api/comment")]
public class CommentController : ApiControllerBase
{

    [HttpGet, Route("")]
    public async Task<ActionResult<ICollection<CommentReadDto>>> GetComments([FromQuery] GetCommentsQuery query)
    {
        return (await Mediator.Send(query)).ToList();
    }

    [HttpPost, Route("")]
    public async Task<ActionResult<CommentReadDto>> AddComment([FromBody] AddCommentCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpDelete, Route("{commentId:int}")]
    public async Task<IActionResult> RemoveComment(int commentId)
    {
        await Mediator.Send(new RemoveCommentCommand { CommentId = commentId });
        return NoContent();
    }

    [HttpPut, Route("")]
    public async Task<ActionResult<CommentReadDto>> UpdateCommentCommand([FromBody] UpdateCommentCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

}