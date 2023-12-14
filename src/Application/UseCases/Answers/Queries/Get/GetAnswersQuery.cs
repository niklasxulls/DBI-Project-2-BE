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
using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Answers;

namespace stackblob.Application.UseCases.Answers.Queries.Get;

public class GetAnswersQuery : IRequest<ICollection<AnswerReadDto>>
{
    public int QuestionId { get; set; }
    public Paging Paging { get; set; } = null!;
    public GetAnswersOrderBy OrderBy { get; set; }
    
}

public class GetAnswersQueryHandler : IRequestHandler<GetAnswersQuery, ICollection<AnswerReadDto>> {
    private readonly IStackblobDbContext _ctx;
    private readonly IMapper _mapper;

    public GetAnswersQueryHandler(IStackblobDbContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }

    public async Task<ICollection<AnswerReadDto>> Handle(GetAnswersQuery request, CancellationToken cancellationToken)
    {
        var query = _ctx.Answers.Where(a => a.QuestionId == request.QuestionId).Skip(request.Paging.Offset).Take(request.Paging.Size);

        switch(request.OrderBy)
        {
            case GetAnswersOrderBy.Newest: query = query.OrderByDescending(a => a.CreatedAt); break;
            case GetAnswersOrderBy.Oldest: query = query.OrderBy(a => a.CreatedAt); break;
        }

        return await query.ProjectTo<AnswerReadDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}
