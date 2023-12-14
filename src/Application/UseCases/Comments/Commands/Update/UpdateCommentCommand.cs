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
using stackblob.Application.Mapping;
using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Answers;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Application.Models.DTOs.Comments;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;

namespace stackblob.Application.UseCases.Comments.Commands.AddQuestion;

[Authorize]
public class UpdateCommentCommand : CommentBaseDto, IRequest<CommentReadDto>, IMapFrom<Comment>
{
    public int CommentId { get; set; }
}

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentReadDto>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCommentCommandHandler(IStackblobDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CommentReadDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        Comment comment = _mapper.Map<Comment>(request);

        Comment currentComment = (await _context.Comments.FindAsync(request.CommentId))!;
        _context.Entry(currentComment).CurrentValues.SetValues(comment);

        await _context.SaveChangesAsync(cancellationToken);


        return _mapper.Map<CommentReadDto>(comment);
    }
}
