using ShopApi.Common;
using System.Net;

namespace ShopApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                var statusCode = ResolveStatusCode(ex);
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";

                var traceId = context.TraceIdentifier;
                var path = context.Request.Path.Value;

                if ((int)statusCode >= 500)
                {
                    _logger.LogError(
                        ex,
                        "Unhandled exception at {Method} {Path}. TraceId: {TraceId}",
                        context.Request.Method,
                        path,
                        traceId);
                }
                else
                {
                    _logger.LogWarning(
                        ex,
                        "Handled exception at {Method} {Path}. StatusCode: {StatusCode}. TraceId: {TraceId}",
                        context.Request.Method,
                        path,
                        (int)statusCode,
                        traceId);
                }

                var response = new ApiErrorResponse
                {
                    Message = ex.InnerException?.Message ?? ex.Message,
                    TraceId = traceId,
                    Path = path
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private static HttpStatusCode ResolveStatusCode(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                return HttpStatusCode.Unauthorized;

            if (ex is KeyNotFoundException)
                return HttpStatusCode.NotFound;

            if (ex is ArgumentException || ex is InvalidOperationException)
                return HttpStatusCode.BadRequest;

            if (ex.Message.Equals("Unauthorized", StringComparison.OrdinalIgnoreCase))
                return HttpStatusCode.Unauthorized;

            if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return HttpStatusCode.NotFound;

            if (ex.Message.Contains("invalid", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("expired", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("empty", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("limit", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("stock", StringComparison.OrdinalIgnoreCase))
                return HttpStatusCode.BadRequest;

            return HttpStatusCode.InternalServerError;
        }
    }
}
