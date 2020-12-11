using System;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Todo.API.Swagger
{
    public class ExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = GetExampleOrNullFor(context.Type);
        }

        private IOpenApiAny GetExampleOrNullFor(Type type)
        {
            return type.Name switch
            {
                "CredentialsDto" => new OpenApiObject
                {
                    ["UserName"] = new OpenApiString("anirudh"),
                    ["Password"] = new OpenApiString("asdf"),
                },
                _ => null,
            };
        }
    }
}
