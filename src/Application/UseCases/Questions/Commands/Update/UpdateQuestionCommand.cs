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
using stackblob.Application.Attributes;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Mapping;

namespace stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;


[Authorize]
public class UpdateQuestionCommand : QuestionWriteDto, IRequest<QuestionReadDto>, IMapFrom<Question>
{
    public int QuestionId { get; set; }

    public new void Mapping(Profile p)
    {
        p.CreateMap<UpdateQuestionCommand, Question>()
         .IncludeBase<QuestionWriteDto, Question>();
    }
}

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionReadDto>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(IStackblobDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<QuestionReadDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        Question question = _mapper.Map<Question>(request);

        Question currentQuestion = (await _context.Questions.FindAsync(request.QuestionId))!;
        _context.Entry(currentQuestion).CurrentValues.SetValues(question);

        await _context.SaveChangesAsync(cancellationToken);


        return _mapper.Map<QuestionReadDto>(question);
    }
}
