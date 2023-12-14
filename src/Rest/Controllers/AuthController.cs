using stackblob.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stackblob.Application.UseCases.Auth.Queries;
using stackblob.Application.UseCases.Auth.Commands.Login;
using stackblob.Application.UseCases.Auth.Commands.Register;
using stackblob.Application.UseCases.Auth.Commands.RefreshTokens;
using stackblob.Application.UseCases.Auth.Commands.Verify;
using stackblob.Application.Models.DTOs.Auth;
using stackblob.Application.UseCases.Auth.Commands.RequestEmailVerification;

namespace stackblob.Rest.Controllers;

[Route("api/auth")]
public class AuthController : ApiControllerBase
{
    [HttpPost, Route("register")]
    public async Task<ActionResult<UserAuthDto>> RegisterUserCommand([FromForm] RegisterUserCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpPost, Route("login")]
    public async Task<ActionResult<UserAuthDto>> Login(LoginUserCommand cmd)
    {
        return await Mediator.Send(cmd);
    }


    [HttpPost, Route("refreshtokens")]
    public async Task<ActionResult<RefreshTokensDto>> RefreshTokens(RefreshTokensCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpPost, Route("verify-email/{verification:Guid}")]
    public async Task<IActionResult> Verify(Guid verification)
    {
        await Mediator.Send(new VerifyUserEmailCommand() { Guid = verification });

        return NoContent();
    }

    [HttpPost, Route("request-verification")]
    public async Task<ActionResult<UserEmailVerificationDto>> RequestEmailVerification()
    {
        return await Mediator.Send(new RequestEmailVerificationCommand() {  });
    }

}
