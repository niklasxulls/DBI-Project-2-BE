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

namespace stackblob.Application.UseCases.Comments.Commands.AddQuestion;

[Authorize]
public class AddCommentCommand : CommentBaseDto, IRequest<CommentReadDto>, IMapFrom<Comment>
{
    public int QuestionId { get; set; }
    public int? AnswerId { get; set; }
}

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentReadDto>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public AddCommentCommandHandler(IStackblobDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    public async Task<CommentReadDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = _mapper.Map<Comment>(request);

        if(request.AnswerId == null)
        {
            comment.CreatedByInAnswerId = _currentUser.UserId;
        } else
        {
            comment.CreatedByInQuestionId = _currentUser.UserId;
        }

        await _context.Comments.AddAsync(comment, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);


        return _mapper.Map<CommentReadDto>(comment);
    }
}
