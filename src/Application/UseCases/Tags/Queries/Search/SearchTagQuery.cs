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

namespace stackblob.Application.UseCases.Tags.Queries.Search;

public class SearchTagQuery : IRequest<ICollection<TagUsageDto>>
{
    public string SearchTerm { get; set; } = string.Empty;
    public Paging Paging { get; set; } = null!;
}

public class SearchTagQueryHandler : IRequestHandler<SearchTagQuery, ICollection<TagUsageDto>>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ICurrentUserService _currentUser;

    public SearchTagQueryHandler(IStackblobDbContext context, IMapper mapper, IFileService fileService, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
        _currentUser = currentUser;
    }
    public async Task<ICollection<TagUsageDto>> Handle(SearchTagQuery request, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags.Where(t => t.Name.Contains(request.SearchTerm)).Select(t => new TagUsageDto()
                                            {
                                                Name = t.Name,
                                                TagId = t.TagId,
                                                UsedCount = t.Questions.Count
                                            })
                                      .Skip(request.Paging.Offset)
                                      .Take(request.Paging.Size)
                                      .ToListAsync(cancellationToken);

        return tags;
    }
}