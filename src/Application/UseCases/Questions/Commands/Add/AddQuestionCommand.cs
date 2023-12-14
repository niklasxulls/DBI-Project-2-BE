using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Questions;

namespace stackblob.Application.UseCases.Questions.Commands.AddQuestion;

[Authorize]
public class AddQuestionCommand : QuestionWriteDto, IRequest<QuestionReadDto>
{
    public ICollection<int>? Attachments { get; set; }
}

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, QuestionReadDto>
{
    private readonly IStackblobDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public AddQuestionCommandHandler(IStackblobDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }
    public async Task<QuestionReadDto> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = _mapper.Map<Question>(request);
        await _context.Questions.AddAsync(question, cancellationToken);

        if (request.Attachments?.Any() ?? false)
        {
        }

        await _context.SaveChangesAsync(cancellationToken);


        return _mapper.Map<QuestionReadDto>(question);
    }
}
