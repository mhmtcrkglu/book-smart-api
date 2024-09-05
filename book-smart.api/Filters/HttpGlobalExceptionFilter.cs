using System.ComponentModel.DataAnnotations;
using System.Net;
using BookSmart.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Npgsql;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<HttpGlobalExceptionFilter> _logger;

    public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Exception occurred: {Message}", context.Exception.Message);
        switch (context.Exception)
        {
            case ValidationException:
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                var exceptionType = context.Exception.GetType().Name;
                problemDetails.Errors.Add($"{exceptionType}", new[] { context.Exception.Message });

                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            }
            case ModelValidationException validationException:
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };
                var exceptionType = context.Exception.GetType().Name;
                problemDetails.Errors.Add($"{exceptionType}", validationException.Errors);
                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            }
            case PostgresException:
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Please refer to the errors property for additional details."
                };
                var exceptionType = context.Exception.GetType().Name;
                problemDetails.Errors.Add($"{exceptionType}", [
                    "An error occurred while processing the request."
                ]);
                context.Result = new InternalServerErrorObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            }
            default:
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Please refer to the errors property for additional details."
                };
                var exceptionType = context.Exception.GetType().Name;
                problemDetails.Errors.Add($"{exceptionType}", [
                    context.Exception.Message
                ]);
                context.Result = new InternalServerErrorObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            }
        }

        context.ExceptionHandled = true;
    }

    private class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}