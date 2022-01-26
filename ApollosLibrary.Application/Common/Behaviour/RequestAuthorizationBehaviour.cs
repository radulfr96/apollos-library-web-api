using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Behaviour
{
    public class RequestAuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IAuthorizer<TRequest>> _authorizers;

        public RequestAuthorizationBehaviour(IEnumerable<IAuthorizer<TRequest>> authorizers)
        {
            _authorizers = authorizers;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            foreach (var authorizer in _authorizers)
            {
                var result = await authorizer.AuthorizeAsync(request, cancellationToken);
                if (!result.IsAuthorized)
                    throw new UnauthorizedException(result.ErrorCode,  result.FailureMessage);
            }

            return await next();
        }
    }
}
