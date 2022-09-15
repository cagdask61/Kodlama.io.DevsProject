using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcers.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext,Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            if (exception.GetType() == typeof(ValidationException))
                return this.CreateValidationException(httpContext, exception);

            if (exception.GetType() == typeof(BusinessException))
                return CreateBusinessException(httpContext, exception);

            if (exception.GetType() == typeof(AuthorizationException))
                return CreateAuthorizationException(httpContext, exception);



            return CreateInternalException(httpContext, exception);
        }

        /*
                     _ = exception.GetType() switch
            {
                Type => this.CreateValidationException(httpContext, exception),
                _ => this.CreateInternalException(httpContext, exception),
            };
         */

        private Task CreateAuthorizationException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
            return context.Response.WriteAsync(new AuthorizationProblemDetails()
            {
                Status = StatusCodes.Status401Unauthorized, 
                Type = "https://kodlarahukmeden.web.app/api-document/problems/authorization",
                Title = "Authorization Exception",
                Detail = exception.Message,
                Instance = "",

            }.ToStringify());
        }

        private Task CreateBusinessException(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

            return httpContext.Response.WriteAsync(new BusinessProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://kodlarahukmeden.web.app/api-document/problems/business",
                Title = "Business Exception",
                Detail = exception.Message,
                Instance = ""

            }.ToStringify());
        }

        private Task CreateValidationException(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            object errors = ((ValidationException)exception).Errors;

            //var result = (IEnumerable<FluentValidation.Results.ValidationFailure>)errors;
            //result.Count();
            
            return httpContext.Response.WriteAsync(new ValidationProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://kodlarahukmeden.web.app/api-document/problems/validation",
                Title = "Validation error(s)",
                Detail = "",
                Instance = "",
                Errors = errors

            }.ToStringify());
        }

        private Task CreateInternalException(HttpContext httpContext, Exception exception)
        {
            //httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.StatusCode =  Convert.ToInt32(HttpStatusCode.InternalServerError);

            return httpContext.Response.WriteAsync(new InternalProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://kodlarahukmeden.web.app/api-document/problems/internal",
                Title = "Internal Exception",
                Detail = exception.Message,
                Instance = ""

            }.ToStringify());
        }
    }
}
