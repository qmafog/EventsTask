using System.Net;
using FluentValidation;
using System.Text.Json;
using EventsTask.Application.Common.Exceptions;
namespace EventsTask.Backend.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (ex)
            {
                case ValidationException valudationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(valudationException.Errors);
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UserVerificationException userVerificationException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case AuthorizeConfigurationException authorizeConfigurationException:
                    code = HttpStatusCode.ServiceUnavailable;
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = ex.Message });
            }
            return context.Response.WriteAsync(result);
        }
    }
}
