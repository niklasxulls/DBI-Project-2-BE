using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.Models.DTOs.Questions;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Models;

namespace stackblob.Application.UseCases.Questions.Queries.Search;

public class GetQuestionsQuery : IRequest<ICollection<QuestionReadShallowDto>>
{
    public string? SearchTerm { get; set; }
    public DateOnly? CreatedAtFrom { get; set; }
    public DateOnly? CreatedAtTill { get; set; }
    public int? CreatedBy { get; set; }
    public ICollection<int>? Tags { get; set; }
    public Paging Paging { get; set; } = null!;
    public SearchQuestionsOrderBy OrderBy { get; set; }
}

public class SearchQuestionQueryHandler : IRequestHandler<GetQuestionsQuery, ICollection<QuestionReadShallowDto>>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ICurrentUserService _currentUser;

    public SearchQuestionQueryHandler(IStackblobDbContext context, IMapper mapper, IFileService fileService, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
        _currentUser = currentUser;
    }
    public async Task<ICollection<QuestionReadShallowDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Questions.AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(q => q.Title.ToLower().Contains(request.SearchTerm.ToLower()));
        }

        if (request.CreatedAtFrom.GetValueOrDefault() != default)
        {
            var createdAtFromDateTime = request.CreatedAtFrom.GetValueOrDefault().ToDateTime(new TimeOnly(0, 0, 0));
            query = query.Where(q => q.CreatedAt >= createdAtFromDateTime);
        }

        if (request.CreatedAtTill.GetValueOrDefault() != default)
        {
            var createdAtTillDateTime = request.CreatedAtTill.GetValueOrDefault().ToDateTime(new TimeOnly(23, 59, 59));
            query = query.Where(q => q.CreatedAt <= createdAtTillDateTime);
        }

        if (request.CreatedBy != null)
        {
            query = query.Where(q => q.CreatedById == request.CreatedBy);
        }

        if(request.Tags?.Any() ?? false)
        {
            query = query.Where(q => q.Tags.All(t => request.Tags.Contains(t.TagId)));
        }

        switch(request.OrderBy)
        {
            case SearchQuestionsOrderBy.Newest: query = query.OrderByDescending(q => q.CreatedAt); break;
            case SearchQuestionsOrderBy.Oldest: query = query.OrderBy(q => q.CreatedAt); break;
            case SearchQuestionsOrderBy.Popularity: query = query.Include(q => q.Answers).Include(q => q.QuestionVotes); break;
        }


        // TODO write custom expression tree in oder to evaluate question popularity on db level
        if(request.OrderBy == SearchQuestionsOrderBy.Popularity)
        {
            var questions = (await query.ToListAsync(cancellationToken)).OrderByDescending(q => q.Popularity).ToList().Skip(request.Paging.Offset).Take(request.Paging.Size);
            return _mapper.Map<List<QuestionReadShallowDto>>(questions);
        } else
        {
            return await query.Skip(request.Paging.Offset)
                              .Take(request.Paging.Size)
                              .ProjectTo<QuestionReadShallowDto>(_mapper.ConfigurationProvider)
                              .ToListAsync(cancellationToken);
        }
    }
}