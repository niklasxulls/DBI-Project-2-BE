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
using stackblob.Application.Models.DTOs.Comments;

namespace stackblob.Application.UseCases.Comments.Queries.Get;
public class GetCommentsQuery : IRequest<ICollection<CommentReadDto>>
{
    public int QuestionId { get; set; }
    public int? AnswerId { get; set; }
    public Paging Paging { get; set; } = null!;
}


public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, ICollection<CommentReadDto>>
{
    private readonly IStackblobDbContext _ctx;
    private readonly IMapper _mapper;

    public GetCommentsQueryHandler(IStackblobDbContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }
    public async Task<ICollection<CommentReadDto>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        return await _ctx.Comments.Where(q => q.QuestionId == request.QuestionId && q.AnswerId == request.AnswerId)
                                   .OrderBy(c => c.CreatedAt)
                                   .Skip(request.Paging.Offset)
                                   .Take(request.Paging.Size)
                                   .ProjectTo<CommentReadDto>(_mapper.ConfigurationProvider)
                                   .ToListAsync(cancellationToken);
    }
}
