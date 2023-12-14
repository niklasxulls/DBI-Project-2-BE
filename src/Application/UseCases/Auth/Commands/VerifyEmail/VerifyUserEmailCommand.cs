using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Interfaces;

namespace stackblob.Application.UseCases.Auth.Commands.Verify;
public class VerifyUserEmailCommand : IRequest
{
    public Guid Guid { get; set; }
}

public class VerifyUserEmailCommandHandler : IRequestHandler<VerifyUserEmailCommand>
{
    private readonly IStackblobDbContext _ctx;

    public VerifyUserEmailCommandHandler(IStackblobDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Unit> Handle(VerifyUserEmailCommand request, CancellationToken cancellationToken)
    {
        var verification = await _ctx.UserEmailVerifications.FirstAsync(u => u.UserEmailVerificationAccess == request.Guid, cancellationToken);

        verification.IsVerified = true;

        await _ctx.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
