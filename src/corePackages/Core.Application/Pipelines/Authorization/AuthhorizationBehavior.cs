using Core.CrossCuttingConcers.Exceptions;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Authorization
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ISecuredRequest
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

            List<string>? roleClaims = HttpContextAccessor.HttpContext.User.ClaimRoles();

            if (roleClaims == null)
                throw new AuthorizationException("Claims not found.");

            bool isNotMatchedRoleClaimWithRequestRoles = roleClaims.FirstOrDefault(roleClaim => request.Roles.Any(role => role == roleClaim)).IsNullOrEmpty();
            if (isNotMatchedRoleClaimWithRequestRoles)
                throw new AuthorizationException("You are not authorized.");

            TResponse response = await next();

            return response;
        }

    }
}
