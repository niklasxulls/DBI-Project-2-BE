using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Mapping;
using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Users;

namespace stackblob.Application.UseCases.Users.Queries.GetById;

[Authorize]
public class GetUserQuery : IRequest<UserDto>
{
    public int UserId { get; set; }
}


public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IMapper _mapper;
    private readonly IStackblobDbContext _context;

    public GetUserQueryHandler(IMapper mapper, IStackblobDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Users.Where(u => u.UserId == request.UserId);


        return await query.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                          .FirstAsync(cancellationToken);
    }
}

