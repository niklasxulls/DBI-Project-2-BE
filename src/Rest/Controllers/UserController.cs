using stackblob.Application.Models;
using stackblob.Application.UseCases.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stackblob.Application.UseCases.Users.Commands;
using stackblob.Application.UseCases.Users.Queries;
using stackblob.Application.Models.DTOs.Users;
using stackblob.Application.UseCases.Users.Commands.Remove;
using stackblob.Application.UseCases.Users.Queries.GetById;
using stackblob.Application.UseCases.Users.Commands.UpdateProfilePicture;
using stackblob.Application.UseCases.Users.Commands.RemoveProfilePicture;

namespace stackblob.Rest.Controllers;

[Route("api/user")]
public class UserController : ApiControllerBase
{


    [HttpPut, Route("profilepicture/update")]
    public async Task<ActionResult<string>> UpdateProfilePictureCommand([FromForm] UpdateProfilePictureCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpDelete, Route("profilepicture/remove")]
    public async Task<ActionResult> RemoveProfilePictureCommand()
    {
        await Mediator.Send(new RemoveProfilePictureCommand());

        return Ok();
    }

    [HttpDelete, Route("remove")]
    public async Task<ActionResult> DeleteUserCommand()
    {
        await Mediator.Send(new RemoveUserCommand());

        return Ok();
    }





    [HttpGet, Route("")]
    public async Task<ActionResult<ICollection<UserDto>>> GetUserQuery([FromQuery] GetUsersQuery query)
    {
        return (await Mediator.Send(query)).ToList();
    }

    [HttpGet, Route("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetUserQuery(int userId)
    {

        return await Mediator.Send(new GetUserQuery() {  UserId = userId });
    }

}
