using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ILoggerService<TRequest> _logger;

    public LoggingBehavior(ILoggerService<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.Log("Logging Pre");
        _logger.Log(_logger.FormatPropsOfObj(request));
        try
        {
            return await next();
        }
        catch (Exception ex) { 
            _logger.Log(ex.Message, LoggingType.Error);
            if(!string.IsNullOrEmpty(ex.StackTrace))
            {
                _logger.Log(ex.StackTrace, LoggingType.Error);
            }

            throw;
        }
    }
}
