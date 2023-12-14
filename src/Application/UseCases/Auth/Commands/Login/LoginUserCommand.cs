using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Mapping;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Models.DTOs.Auth;

namespace stackblob.Application.UseCases.Auth.Commands.Login;

public class LoginUserCommand : IRequest<UserAuthDto>, IMapFrom<User>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserAuthDto>
{
    private readonly IAuthService _auth;
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IIPAddressResolverService _ipAddressResolver;

    public LoginUserCommandHandler(IAuthService auth, IStackblobDbContext context, IMapper mapper, ICurrentUserService currentUser, IIPAddressResolverService iPAddressResolver)
    {
        _auth = auth;
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
        _ipAddressResolver = iPAddressResolver;
    }

    public async Task<UserAuthDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Where(u => u.Email.ToLower().Equals(request.Email.ToLower()))
                                        .Include(u => u.Socials)
                                        .FirstAsync(cancellationToken);

        var userDto = _mapper.Map<UserAuthDto>(user);

        var (accessToken, expiresAt) = _auth.GenerateAccessToken(userDto);
        var refreshToken = _auth.GenerateRefreshToken();
        refreshToken.UserID = user.UserId;

        _context.RefreshTokens.Add(refreshToken);

        userDto.AccessToken = accessToken;
        userDto.ExpiresAt = expiresAt;
        userDto.RefreshToken = refreshToken.Token;

        if (_currentUser.IpAddress != null)
        {
            var location = await _ipAddressResolver.ResolveLocationDetailsByIPAddress(_currentUser.IpAddress);
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Abbreviation == location.CountryCode);

            if(location != null && country != null)
            {
                user.LoginLocations.Add(new LoginLocation()
                {
                    Country = country,
                    IpAddress = _currentUser.IpAddress,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    TimeZoneId = null!
                });
            }
        }


        await _context.SaveChangesAsync(cancellationToken);

        return userDto;
    }
}
