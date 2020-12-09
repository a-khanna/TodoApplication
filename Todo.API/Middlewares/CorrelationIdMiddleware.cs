using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NLog;
using Todo.Core;

namespace Todo.API.Middlewares
{
    /// <summary>
    /// Adds x-correlation-id to the request and response.
    /// If the correlation id header already exists, passes it on to the response.
    /// </summary>
    public class CorrelationIdMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string correlationId = context.Request.Headers.TryGetValue(Constants.XCorrelationId, out var correlationIds) ? correlationIds.FirstOrDefault() : Guid.NewGuid().ToString();

            // Set correlation id to be included in the log messages
            MappedDiagnosticsLogicalContext.Set("CorrelationId", correlationId);

            context.Request.Headers.Add(Constants.XCorrelationId, correlationId);
            context.Response.OnStarting(state => {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add(Constants.XCorrelationId, correlationId);
                return Task.CompletedTask;
            }, context);

            await next(context);
        }
    }
}
