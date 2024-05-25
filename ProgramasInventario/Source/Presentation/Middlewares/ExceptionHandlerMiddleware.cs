using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;

namespace Presentation.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            //context.Response.ContentType = "application/json";
            switch (e)
            {
                case InvalidEmailFormatException:
                case InvalidPasswordException:
                case InvalidPasswordFormatException:
                    _logger.LogWarning(e, $"Expected error: {e.Message}");
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException:
                    _logger.LogWarning(e, $"Expected error: {e.Message}");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case UserAlreadyRegisteredException:
                    _logger.LogWarning(e, $"Expected error: {e.Message}");
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                case UserNotFoundException:
                    _logger.LogWarning(e, $"Expected error: {e.Message}");
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    _logger.LogError(e, $"Unexpected error: {e.Message}");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { e.Message }));
        }
    }
}