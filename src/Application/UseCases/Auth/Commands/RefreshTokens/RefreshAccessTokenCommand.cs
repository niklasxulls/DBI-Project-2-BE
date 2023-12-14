using stackblob.Application.Interfaces.Services;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces;
using AutoMapper.QueryableExtensions;
using stackblob.Application.Models.DTOs.Auth;

namespace stackblob.Application.UseCases.Auth.Commands.RefreshTokens
{
    public class RefreshTokensCommand : IRequest<RefreshTokensDto>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokensCommandCommandHandler : IRequestHandler<RefreshTokensCommand, RefreshTokensDto>
    {
        private readonly IAuthService _auth;
        private readonly IMapper _mapper;
        private readonly IStackblobDbContext _context;

        public RefreshTokensCommandCommandHandler(IAuthService auth, IMapper mapper, IStackblobDbContext context)
        {
            _auth = auth;
            _mapper = mapper;
            _context = context;
        }

        public async Task<RefreshTokensDto> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
        {
            var userId = _auth.GetUsedIdByAccessToken(request.AccessToken);


            var authUser = await _context.Users.Where(u => u.UserId == userId).ProjectTo<UserAuthDto>(_mapper.ConfigurationProvider).FirstAsync(cancellationToken);
            var currtoken = await _context.RefreshTokens.FirstAsync(r => r.Token == request.RefreshToken && r.UserID == authUser.UserId, cancellationToken);


            if (currtoken.AlreadyUsed || currtoken.ExpiresAt < DateTimeUtil.Now())
            {
                _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(r => r.UserID == authUser.UserId));
                await _context.SaveChangesAsync(cancellationToken);
                throw new ForbiddenAccessException("RefreshToken already used or expired!");
            }
            else
            {
                currtoken.AlreadyUsed = true;
            }

            var (accessToken, expiresAt) = _auth.GenerateAccessToken(authUser);
            var refreshToken = _auth.GenerateRefreshToken();

            refreshToken.UserID = authUser.UserId;
            _context.RefreshTokens.Add(refreshToken);

            authUser.AccessToken = accessToken;
            authUser.ExpiresAt = expiresAt;
            authUser.RefreshToken = refreshToken.Token;


            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RefreshTokensDto>(authUser);
        }
    }
}
