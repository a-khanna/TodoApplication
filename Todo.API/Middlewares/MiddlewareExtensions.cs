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

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseContentLocation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ContentLocationMiddleware>();
        }

        public static IApplicationBuilder UseGraphQLJWTMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GraphQLJWTMiddleware>();
        }

        public static void RegisterMiddlewares(this IServiceCollection services)
        {
            services.AddSingleton<CorrelationIdMiddleware>();
            services.AddSingleton<RequestLoggingMiddleware>();
            services.AddSingleton<ExceptionHandlingMiddleware>();
            services.AddSingleton<ContentLocationMiddleware>();
            services.AddSingleton<GraphQLJWTMiddleware>();
        }
    }

}
