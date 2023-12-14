using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;

namespace stackblob.Application.UseCases.Auth.Queries;

public class EmailExistsQuery : IRequest<bool>
{
    public string SearchTerm { get; set; } = string.Empty;
}

public class EmailExistsQueryHandler : IRequestHandler<EmailExistsQuery, bool>
{
    private readonly IStackblobDbContext _context;

    public EmailExistsQueryHandler(IStackblobDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(EmailExistsQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = request.SearchTerm.ToLower();
        return await _context.Users.AnyAsync(user => user.Email.ToLower() == searchTerm);
    }
}

