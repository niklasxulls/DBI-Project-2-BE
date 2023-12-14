using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Attributes;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Models.DTOs.Auth;
using stackblob.Domain.Util;

namespace stackblob.Application.UseCases.Auth.Commands.RequestEmailVerification;

[Authorize(allowUnverified: true)]
public class RequestEmailVerificationCommand : IRequest<UserEmailVerificationDto>
{
}


public class RequestEmailVerificationCommandHandler : IRequestHandler<RequestEmailVerificationCommand, UserEmailVerificationDto>
{
    private readonly IStackblobDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;

    public RequestEmailVerificationCommandHandler(IStackblobDbContext context, ICurrentUserService currentUser, IMapper mapper, IMailService mailService)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
        _mailService = mailService;
    }

    public async Task<UserEmailVerificationDto> Handle(RequestEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        var existingVerficiations = await _context.UserEmailVerifications.Where(u => u.UserId == _currentUser.UserId).ToListAsync(cancellationToken);
        var user = await _context.Users.FirstAsync(u => u.UserId == _currentUser.UserId, cancellationToken);

        existingVerficiations.ForEach(v => v.ExpiresAt = DateTimeUtil.Now());

        var verification = new UserEmailVerification()
        {
            ExpiresAt = DateTimeUtil.Now().AddDays(1),
            UserId = _currentUser.UserId ?? 0,
        };

        _context.UserEmailVerifications.Add(verification);

        await _context.SaveChangesAsync(cancellationToken);

        await _mailService.SendEmailVerification(user, verification.UserEmailVerificationAccess);

        return _mapper.Map<UserEmailVerificationDto>(verification);
    }
}
