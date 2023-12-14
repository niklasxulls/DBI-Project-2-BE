using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Application.Attributes;
using stackblob.Application.Models.DTOs.Tags;

namespace stackblob.Application.UseCases.Tags.Commands.Add;


[Authorize]
public class AddTagCommand : IRequest<TagReadDto>
{
    public string Name { get; set; } = string.Empty;
}

public class CreateTagCommandHandler : IRequestHandler<AddTagCommand, TagReadDto>
{
    private readonly IStackblobDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ICurrentUserService _currentUser;

    public CreateTagCommandHandler(IStackblobDbContext context, IMapper mapper, IFileService fileService, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
        _currentUser = currentUser;
    }
    public async Task<TagReadDto> Handle(AddTagCommand request, CancellationToken cancellationToken)
    {
        var tag = new Tag() { Name = StringUtil.ReplaceSpecialCharacters(request.Name) };
       
        await _context.Tags.AddAsync(tag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TagReadDto>(tag);
    }
}