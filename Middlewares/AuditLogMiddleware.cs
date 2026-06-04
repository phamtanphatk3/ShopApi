using ShopApi.Data;
using ShopApi.Models;
using System.Security.Claims;

namespace ShopApi.Middlewares
{
    public class AuditLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditLogMiddleware> _logger;

        public AuditLogMiddleware(RequestDelegate next, ILogger<AuditLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (!ShouldAudit(context))
                return;

            try
            {
                var db = context.RequestServices.GetRequiredService<AppDbContext>();
                db.AuditLogs.Add(new AuditLog
                {
                    UserId = TryGetUserId(context),
                    Username = context.User.Identity?.Name,
                    Role = context.User.FindFirstValue(ClaimTypes.Role),
                    Action = $"{context.Request.Method} {context.Request.Path}",
                    EntityName = ResolveEntityName(context.Request.Path.Value),
                    EntityId = ResolveEntityId(context.Request.Path.Value),
                    Method = context.Request.Method,
                    Path = context.Request.Path.Value ?? string.Empty,
                    StatusCode = context.Response.StatusCode,
                    IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                    CreatedAt = DateTime.UtcNow
                });

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Khong the ghi audit log cho {Path}", context.Request.Path);
            }
        }

        private static bool ShouldAudit(HttpContext context)
        {
            if (HttpMethods.IsGet(context.Request.Method) || HttpMethods.IsHead(context.Request.Method) ||
                HttpMethods.IsOptions(context.Request.Method))
                return false;

            var path = context.Request.Path.Value ?? string.Empty;
            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
                return false;

            if (path.StartsWith("/api/health", StringComparison.OrdinalIgnoreCase))
                return false;

            return HttpMethods.IsPost(context.Request.Method) ||
                   HttpMethods.IsPut(context.Request.Method) ||
                   HttpMethods.IsPatch(context.Request.Method) ||
                   HttpMethods.IsDelete(context.Request.Method);
        }

        private static int? TryGetUserId(HttpContext context)
        {
            var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }

        private static string? ResolveEntityName(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length < 2)
                return null;

            return segments[1];
        }

        private static string? ResolveEntityId(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return segments.LastOrDefault(segment => int.TryParse(segment, out _));
        }
    }
}
