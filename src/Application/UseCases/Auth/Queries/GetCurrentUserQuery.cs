using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Models.DTOs.Users;

namespace stackblob.Application.UseCases.Auth.Queries;

public class GetCurrentUserQuery : IRequest<UserDto>
{
}

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public GetCurrentUserQueryHandler(ICurrentUserService currentUser, IStackblobDbContext context, IMapper mapper, IFileService fileService)
    {
        _currentUser = currentUser;
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                        .FirstAsync(u => u.UserId == _currentUser.UserId);
        return user;
    }
}
