using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Services;

namespace AcademicRiskRadar.Web.Middleware;

/// <summary>
/// Unit IV: Middleware Request Pipeline - Custom Middleware 2
/// Logs security actions and tracks authorized/anonymous access.
/// Demonstrates scoped dependency resolution inside middleware via HttpContext.RequestServices.
/// </summary>
public class AcademicSecurityAuditMiddleware
{
    private readonly RequestDelegate _next;

    public AcademicSecurityAuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Execute the rest of the pipeline first
        await _next(context);

        // Filter out static assets (.css, .js, .png, etc.) to keep audit log focused on API & MVC endpoints
        string path = context.Request.Path.Value ?? "";
        if (path.StartsWith("/lib") || path.StartsWith("/css") || path.EndsWith(".png") || path.EndsWith(".ico") || path.EndsWith(".js") || path.EndsWith(".css"))
        {
            return;
        }

        try
        {
            // Resolve scoped repository safely from request services
            var repository = context.RequestServices.GetService(typeof(IStudentRepository)) as IStudentRepository;
            if (repository != null)
            {
                string user = context.User.Identity?.IsAuthenticated == true
                    ? context.User.Identity.Name ?? "Authenticated User"
                    : "Anonymous Guest";

                string role = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "Guest";
                string ip = context.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

                repository.AddAuditLog(new AuditLogEntry
                {
                    HttpMethod = context.Request.Method,
                    Path = path,
                    StatusCode = context.Response.StatusCode,
                    ClientIp = ip,
                    UserIdentity = user,
                    Role = role,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
        catch
        {
            // Middleware should never suppress or crash the primary pipeline
        }
    }
}
