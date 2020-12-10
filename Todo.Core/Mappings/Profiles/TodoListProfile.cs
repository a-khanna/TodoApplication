using System.Collections.Generic;
using AutoMapper;
using Todo.Core.MappingProfiles.Converters;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Response;
using Todo.Core.Models.Sql;

namespace Todo.Core.Mappings.Profiles
{
    public class TodoListProfile : Profile
    {
        public TodoListProfile()
        {
            CreateMap<TodoList, TodoListDetailedDto>();
            CreateMap<TodoList, TodoListDto>();
            CreateMap<PagedResult<TodoList>, PagedResult<TodoListDto>>();
            CreateMap<List<Label>, List<string>>();
            CreateMap<Label, string>().ConvertUsing<LabelToStringConverter>();
        }
    }
}
