
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.Attributes;
using stackblob.Application.Models.DTOs.Questions;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace stackblob.Application.UseCases.Questions.Queries.GetById;


public class GetQuestionQuery : IRequest<QuestionReadDto>
{
    public Guid QuestionIdAccess { get; set; }
}

public class GetQuestionCommandHandler : IRequestHandler<GetQuestionQuery, QuestionReadDto>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetQuestionCommandHandler(IStackblobDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    public async Task<QuestionReadDto> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        QuestionReadDto? question = await _context.Questions.Where(f => f.QuestionIdAccess == request.QuestionIdAccess)
                                                            .ProjectTo<QuestionReadDto>(_mapper.ConfigurationProvider)
                                                            .FirstAsync(cancellationToken);
        return question;
    }
}