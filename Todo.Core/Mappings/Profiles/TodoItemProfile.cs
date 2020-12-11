using AutoMapper;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Response;
using Todo.Core.Models.Sql;

namespace Todo.Core.Mappings.Profiles
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemDto>();
            CreateMap<PagedResult<TodoItem>, PagedResult<TodoItemDto>>();

            CreateMap<UpdateTodoItemDto, TodoItemDto>();
        }
    }
}
