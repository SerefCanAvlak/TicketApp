using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Ticketing.WebAPI.Middlewares
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";

            if (exception is ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = validationException.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });

                var response = new
                {
                    Title = "Validation Error",
                    Status = 400,
                    Errors = errors
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response), cancellationToken);
                return true;
            }

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problem = new
            {
                Title = "Internal Server Error",
                Status = 500,
                Detail = exception.Message
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problem), cancellationToken);
            return true;
        }
    }
}
