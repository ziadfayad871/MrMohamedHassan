using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Middleware;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;

    public AuditLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method != "GET")
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            context.Request.EnableBuffering();
            var body = string.Empty;
            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            using var scope = context.RequestServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.AuditLogs.Add(new AuditLog
            {
                UserId = userId,
                Action = context.Request.Method,
                EntityName = context.Request.Path,
                IPAddress = ipAddress,
                NewValues = body.Length > 2000 ? body[..2000] : body,
                Timestamp = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
        }

        await _next(context);
    }
}
