using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Todo.Core.Models.Response;

namespace Todo.API.Middlewares
{
    /// <summary>
    /// Middleware to grab all exceptions and send a standard response model.
    /// Also logs the exception.
    /// </summary>
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {           
            var originalResponseStream = context.Response.Body;

            using (var substituteStream = new MemoryStream())
            {
                context.Response.Body = substituteStream;

                try
                {
                    await next(context);
                }
                catch (Exception ex)
                {
                    var errorModel = new ErrorResponse
                    {
                        Status = context.Response.StatusCode,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    };

                    await substituteStream.CopyToAsync(originalResponseStream);
                    context.Response.Body = originalResponseStream;

                    logger.LogError(ex, "Exception occured.");

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(errorModel));
                }                

                await substituteStream.CopyToAsync(originalResponseStream);
                context.Response.Body = originalResponseStream;
            }
        }
    }
}
