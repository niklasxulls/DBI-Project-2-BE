using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces.Services;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using stackblob.Application.Attributes;

namespace stackblob.Application.Behaviours
{
    public class AuthorizationBehavior<TRequest> : IRequestPreProcessor<TRequest> where TRequest : class
    {
        private readonly ICurrentUserService _currentUser;

        public AuthorizationBehavior(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        { 
            var auth = request.GetType().GetCustomAttribute<Authorize>();
            if (auth != null)
            {
                if(_currentUser.UserId == null)
                {
                    throw new ForbiddenAccessException("Bearer Token is missing, invalid or wrong!");
                }
                if(!auth.AllowUnverified && !_currentUser.IsVerified) {
                    throw new ForbiddenAccessException("User needs to be verified to execute this action!");
                }
            }
            await Task.FromResult(0);   
        }
    }
}
