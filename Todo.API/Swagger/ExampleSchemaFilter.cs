using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;

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
                nameof(ProblemDetails) => new OpenApiObject
                {
                    [nameof(ProblemDetails.Type)] = new OpenApiString("Type of error"),
                    [nameof(ProblemDetails.Title)] = new OpenApiString("Error title"),
                    [nameof(ProblemDetails.Status)] = new OpenApiInteger(401),
                    [nameof(ProblemDetails.Detail)] = new OpenApiString("Error Detail"),
                    [nameof(ProblemDetails.Instance)] = new OpenApiString("Instance name"),
                },
                nameof(ErrorResponse) => new OpenApiObject
                {
                    [nameof(ErrorResponse.Status)] = new OpenApiBoolean(true),
                    [nameof(ErrorResponse.Message)] = new OpenApiString("Error description."),
                    [nameof(ErrorResponse.StackTrace)] = new OpenApiString("Error stacktrace in case of development environment"),
                },
                nameof(TodoListDto) => new OpenApiObject
                {
                    [nameof(TodoListDto.Id)] = new OpenApiInteger(1),
                    [nameof(TodoListDto.Name)] = new OpenApiString("Shopping list"),
                },
                nameof(TodoListDetailedDto) => new OpenApiObject
                {
                    [nameof(TodoListDetailedDto.Id)] = new OpenApiInteger(1),
                    [nameof(TodoListDetailedDto.Name)] = new OpenApiString("Shopping list"),
                    [nameof(TodoListDetailedDto.Labels)] = new OpenApiArray { new OpenApiString("Urgent"), new OpenApiString("Overdue") },
                },
                nameof(UpdateTodoListDto) => new OpenApiObject
                {
                    [nameof(UpdateTodoListDto.Id)] = new OpenApiInteger(1),
                    [nameof(UpdateTodoListDto.Name)] = new OpenApiString("Shopping list"),
                },
                nameof(CreateTodoListDto) => new OpenApiObject
                {
                    [nameof(CreateTodoListDto.Name)] = new OpenApiString("Christmas gifts")
                },
                nameof(TodoItemDto) => new OpenApiObject
                {
                    [nameof(TodoItemDto.Id)] = new OpenApiInteger(1),
                    [nameof(TodoItemDto.Description)] = new OpenApiString("Cheese"),
                },
                nameof(UpdateTodoItemDto) => new OpenApiObject
                {
                    [nameof(UpdateTodoItemDto.Id)] = new OpenApiInteger(1),
                    [nameof(UpdateTodoItemDto.Description)] = new OpenApiString("Cheese"),
                },
                nameof(CreateTodoItemDto) => new OpenApiObject
                {
                    [nameof(CreateTodoItemDto.TodoListId)] = new OpenApiInteger(1),
                    [nameof(CreateTodoItemDto.Description)] = new OpenApiString("Cheese")
                },
                nameof(LabelDto) => new OpenApiObject
                {
                    [nameof(LabelDto.Id)] = new OpenApiInteger(1),
                    [nameof(LabelDto.Name)] = new OpenApiString("Urgent"),
                },
                nameof(CreateLabelDto) => new OpenApiObject
                {
                    [nameof(CreateLabelDto.ParentId)] = new OpenApiInteger(1),
                    [nameof(CreateLabelDto.Label)] = new OpenApiString("Urgent"),
                },
                nameof(DeleteLabelDto) => new OpenApiObject
                {
                    [nameof(DeleteLabelDto.ParentId)] = new OpenApiInteger(1),
                    [nameof(DeleteLabelDto.Label)] = new OpenApiString("Urgent"),
                },
                nameof(UpdateLabelDto) => new OpenApiObject
                {
                    [nameof(UpdateLabelDto.ParentId)] = new OpenApiInteger(1),
                    [nameof(UpdateLabelDto.CurrentValue)] = new OpenApiString("Today"),
                    [nameof(UpdateLabelDto.NewValue)] = new OpenApiString("Tomorrow"),
                },
                nameof(CredentialsDto) => new OpenApiObject
                {
                    [nameof(CredentialsDto.Username)] = new OpenApiString("anirudh"),
                    [nameof(CredentialsDto.Password)] = new OpenApiString("asdf"),
                },
                nameof(UserDto) => new OpenApiObject
                {
                    [nameof(UserDto.Username)] = new OpenApiString("peter1"),
                    [nameof(UserDto.Password)] = new OpenApiString("asdf"),
                    [nameof(UserDto.FirstName)] = new OpenApiString("Peter"),
                    [nameof(UserDto.LastName)] = new OpenApiString("Smith")
                },
                _ => null,
            };
        }
    }
}
