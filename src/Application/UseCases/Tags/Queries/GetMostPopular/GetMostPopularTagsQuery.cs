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

namespace stackblob.Application.UseCases.Tags.Queries.GetMostPopular;

public class GetMostPopularTagsQuery : IRequest<ICollection<TagUsageDto>>
{
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public Paging Paging { get; set; } = null!;
}

public class GetMostPopularTagsQueryHandler : IRequestHandler<GetMostPopularTagsQuery, ICollection<TagUsageDto>>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ICurrentUserService _currentUser;

    public GetMostPopularTagsQueryHandler(IStackblobDbContext context, IMapper mapper, IFileService fileService, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
        _currentUser = currentUser;
    }
    public async Task<ICollection<TagUsageDto>> Handle(GetMostPopularTagsQuery request, CancellationToken cancellationToken)
    {
        var startDateTime = request.Start.ToDateTime(new TimeOnly(0, 0, 0));
        var endDateTime = request.Start.ToDateTime(new TimeOnly(23, 59, 59));


        var tags = await _context.Tags.Select(t => new TagUsageDto()
                                {
                                    Name = t.Name,
                                    TagId = t.TagId,
                                    UsedCount = t.Questions.Where(q => q.UpdatedAt >= startDateTime && q.UpdatedAt <= endDateTime).Count()
                                })
                                .Skip(request.Paging.Offset)
                                .Take(request.Paging.Size)
                                .ToListAsync(cancellationToken);

        return tags;
    }
}