using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Todo.API.Middlewares
{
    /// <summary>
    /// Adds content location header to the responses
    /// </summary>
    public class ContentLocationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                int responseStatusCode = context.Response.StatusCode;
                if (responseStatusCode == StatusCodes.Status201Created)
                {
                    var headers = context.Response.Headers;
                    if (headers.TryGetValue("Content-Location", out StringValues locationHeaderValue))
                    {
                        context.Response.Headers.Remove("Content-Location");
                        context.Response.Headers.Add("Content-Location", context.Response.Headers["Location"]);
                    }
                    else
                    {
                        context.Response.Headers.Add("Content-Location", context.Response.Headers["Location"]);
                    }
                }
                return Task.CompletedTask;
            });
            await next(context);
        }
    }
}
