using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Todo.API.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }

        public static void RegisterMiddlewares(this IServiceCollection services)
        {
            services.AddSingleton<CorrelationIdMiddleware>();
            services.AddSingleton<RequestLoggingMiddleware>();
        }
    }

}
