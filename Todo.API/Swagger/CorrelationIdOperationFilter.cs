using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Todo.Core;

namespace Todo.API.Swagger
{
    /// <summary>
    /// Allows x-correlation-id to be passed in the headers using the swagger UI
    /// </summary>
    public class CorrelationIdOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = Constants.XCorrelationId,
                In = ParameterLocation.Header,
                Description = "Correlation id (Guid) to be passed to the request. Useful for tracking in the logs.",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "uuid"
                }
            });
        }
    }
}
