using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace GlobalPay.Logging.Middlewares
{
    public class LoggingEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingEnrichmentMiddleware> _logger;

        public LoggingEnrichmentMiddleware(RequestDelegate next, ILogger<LoggingEnrichmentMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var cid)
                ? cid.ToString()
                : Guid.NewGuid().ToString();

            var userId = context.User?.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "anonymous";

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
            using (LogContext.PushProperty("UserId", userId))
            {
                context.Response.Headers["X-Correlation-ID"] = correlationId;
                await _next(context);
            }
        }
    }
}
