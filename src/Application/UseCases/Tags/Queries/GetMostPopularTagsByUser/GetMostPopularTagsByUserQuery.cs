using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.Models.DTOs.Tags;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Models;

namespace stackblob.Application.UseCases.Tags.Queries.GetMostPopularTagsByUser;

public class GetMostPopularTagsByUserQuery : IRequest<ICollection<TagUsageDto>>
{
    public int UserId { get; set; }
    public Paging Paging { get; set; } = null!;
}

public class GetMostPopularTagsByUserQueryHandler : IRequestHandler<GetMostPopularTagsByUserQuery, ICollection<TagUsageDto>>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetMostPopularTagsByUserQueryHandler(IStackblobDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    public async Task<ICollection<TagUsageDto>> Handle(GetMostPopularTagsByUserQuery request, CancellationToken cancellationToken)
    {


        var tags = await _context.Tags.Select(t => new TagUsageDto()
                                {
                                    Name = t.Name,
                                    TagId = t.TagId,
                                    UsedCount = t.Questions.Where(q => q.CreatedById == request.UserId).Count()
                                })
                                .Skip(request.Paging.Offset)
                                .Take(request.Paging.Size)
                                .ToListAsync(cancellationToken);

        return tags;
    }
}