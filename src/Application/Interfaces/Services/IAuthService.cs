using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Auth;
using stackblob.Application.UseCases.Auth.Commands.Login;
using stackblob.Application.UseCases.Auth.Commands.RefreshTokens;
using stackblob.Application.UseCases.Auth.Commands.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Interfaces.Services;

public interface IAuthService
{
    //Task<UserAuthDto> Authenticate(LoginUserCommand login, CancellationToken cancellationToken = default);
    //bool Authorize(string token);
    RegisterUserDto GenerateCredentials(RegisterUserCommand user, CancellationToken cancellationToken = default);
    //Task<UserAuthDto> RefreshTokens(RefreshTokensCommand cmd, CancellationToken cancellationToken);
    bool PasswordsEqual(string plainPassword, string salt, string hashedPassword);
    Tuple<string, DateTime> GenerateAccessToken(UserAuthDto user);
    RefreshToken GenerateRefreshToken();
    int? GetUsedIdByAccessToken(string accessToken);
}
