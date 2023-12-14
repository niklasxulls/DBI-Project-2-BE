using stackblob.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveAttachment;
using stackblob.Application.UseCases.Questions.Queries.Search;
using stackblob.Application.UseCases.Tags.Queries.Search;
using stackblob.Application.Models.DTOs.Tags;
using stackblob.Application.UseCases.Tags.Commands.Add;

namespace stackblob.Rest.Controllers;

[Route("api/tag")]
public class TagController : ApiControllerBase
{

    [HttpGet, Route("")]
    public async Task<ActionResult<ICollection<TagUsageDto>>> GetTags([FromQuery] SearchTagQuery query)
    {
        return (await Mediator.Send(query)).ToList();
    }

    [HttpPost, Route("")]
    public async Task<ActionResult<TagReadDto>> AddTag([FromBody] AddTagCommand cmd)
    {
        return await Mediator.Send(cmd);
    }



}
