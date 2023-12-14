using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Mapping;
using stackblob.Application.Models;
using AutoMapper;
using stackblob.Domain.Entities;
using stackblob.Domain.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using stackblob.Domain.Util;
using DateTimeUtil = stackblob.Domain.Util.DateTimeUtil;
using stackblob.Application.UseCases.Auth.Commands.Login;
using stackblob.Application.UseCases.Auth.Commands.Register;
using stackblob.Application.UseCases.Auth.Commands.RefreshTokens;
using Microsoft.Extensions.Azure;
using stackblob.Application.Models.DTOs.Auth;
using stackblob.Infrastructure.Util;

namespace stackblob.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AuthSettings _authSettings;
    private readonly IStackblobDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IIPAddressResolverService _ipAddressResolver;
    private readonly IMapper _mapper;
    private readonly TokenValidationParameters _validationParameter;

    public AuthService(IOptions<AuthSettings> authSettings, 
                       IStackblobDbContext context, 
                       ICurrentUserService currentUser,
                       IIPAddressResolverService ipAddressResolver,
                       IMapper _mapper, TokenValidationParameters validationParameter)
    {
        _authSettings = authSettings.Value;
        _context = context;
        _currentUser = currentUser;
        _ipAddressResolver = ipAddressResolver;
        this._mapper = _mapper;
        _validationParameter = validationParameter;
    }

    public RegisterUserDto GenerateCredentials(RegisterUserCommand register, CancellationToken cancellationToken)
    {
        var user = new RegisterUserDto() { };

        user.Salt = CryptoUtil.CreateSalt();
        user.HashedPassword = CryptoUtil.CreateHash(user.Salt + register.Password);

        return user;
    }

    public bool PasswordsEqual(string plainPassword, string salt, string hashedPassword)
    {
        return hashedPassword == CryptoUtil.CreateHash(salt + plainPassword);
    }

    public int? GetUsedIdByAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParams = _validationParameter.Clone();
        validationParams.ValidateLifetime = false;
        //abstract possible JWT exception...
        try
        {
            var principle = tokenHandler.ValidateToken(accessToken, validationParams, out var securityToken);

            if(securityToken == null || principle == null)
            {
                throw new Exception("");
            }

            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                if (principle.FindFirst(AuthClaimSettings.USERID_CLAIM_NAME)?.Value == null)
                    return null;
                int? id = int.Parse(principle.FindFirst(AuthClaimSettings.USERID_CLAIM_NAME)?.Value ?? "");
                return id;
            }
        } catch(Exception ex)
        {
            throw new ForbiddenAccessException("Invalid token provided");
        }
        return null;
    }

    public Tuple<string, DateTime> GenerateAccessToken(UserAuthDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_authSettings.SecretKey);
        var claims = new Dictionary<string, object>();
        //claims.Add(GlobalConfig.USERNAME_CLAIM_NAME, user.Name);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(AuthClaimSettings.USERID_CLAIM_NAME, Convert.ToString(user.UserId)),
                new Claim(AuthClaimSettings.USERISVERIFIED_CLAIM_NAME, Convert.ToString(user.IsVerified))

            }),
            Claims = claims,
            Expires = DateTimeUtil.Now().AddMinutes(_authSettings.Expires),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256),
            Audience = _authSettings.Audience,
            Issuer = _authSettings.Issuer,
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Tuple.Create(tokenHandler.WriteToken(token), (DateTime) tokenDescriptor.Expires);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var hash = CryptoUtil.CreateHash();
        return new RefreshToken()
        {
            Token = hash,
            ExpiresAt = DateTimeUtil.Now().AddDays(_authSettings.RefreshTokenExpires).AddHours(
            DateTimeUtil.Now().Hour > 3 ? (24 - DateTime.UtcNow.Hour) + 3 : 3 - DateTime.UtcNow.Hour)
            .AddMinutes(Random.Shared.NextInt64(0, 60))
        };
    }




}
