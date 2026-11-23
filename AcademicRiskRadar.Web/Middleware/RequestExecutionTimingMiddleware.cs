using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AcademicRiskRadar.Web.Middleware;

/// <summary>
/// Unit IV: Middleware Request Pipeline - Custom Middleware 1
/// Measures request execution duration and adds an HTTP response header.
/// Demonstrates async request delegate handling and short-circuit capability.
/// </summary>
public class RequestExecutionTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestExecutionTimingMiddleware> _logger;

    public RequestExecutionTimingMiddleware(RequestDelegate next, ILogger<RequestExecutionTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Hook into response starting to append custom diagnostic header
        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            long elapsedMs = stopwatch.ElapsedMilliseconds;
            context.Response.Headers["X-Response-Time-Ms"] = elapsedMs.ToString();
            return Task.CompletedTask;
        });

        // Call the next middleware in the pipeline
        await _next(context);

        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
        }

        _logger.LogInformation(
            "HTTP {Method} {Path} responded {StatusCode} in {Elapsed} ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds
        );
    }
}
