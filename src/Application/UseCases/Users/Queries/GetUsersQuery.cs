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

namespace stackblob.Application.UseCases.Users.Queries;

[Authorize]
public class GetUsersQuery : IRequest<ICollection<UserDto>>
{
    public string? SearchTerm { get; set; }
    public Paging Paging { get; set; } = null!;
}


public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ICollection<UserDto>>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IStackblobDbContext _context;

    public GetUsersQueryHandler(IMediator mediator, IMapper mapper, IStackblobDbContext context)
    {
        _mediator = mediator;
        _mapper = mapper;
        _context = context;
    }
    public async Task<ICollection<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(u => (u.Firstname + " " + u.Lastname).ToLower().Contains(request.SearchTerm.ToLower()));
        }
        

        return await query.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                          .Skip(request.Paging.Offset)
                          .Take(request.Paging.Size)
                          .ToListAsync(cancellationToken);
    }
}

