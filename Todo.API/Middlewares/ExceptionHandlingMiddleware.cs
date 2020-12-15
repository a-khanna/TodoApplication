using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
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
        private readonly IWebHostEnvironment env;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
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
                        Status = false,
                        Message = ex.Message,
                    };

                    if (env.IsDevelopment())
                    {
                        errorModel.StackTrace = ex.StackTrace;
                    }

                    await substituteStream.CopyToAsync(originalResponseStream);
                    context.Response.Body = originalResponseStream;

                    logger.LogError(ex, "Exception occured.");

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(errorModel));
                }  
                
                if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                    logger.LogError("Bad request occurred! Check response log for more details.");

                await substituteStream.CopyToAsync(originalResponseStream);
                context.Response.Body = originalResponseStream;
            }
        }
    }
}
