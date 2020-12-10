using System.Collections.Generic;

namespace Todo.Core.Models.Dtos
{
    /// <summary>
    /// Detailed Todo List Dto containing all the properties
    /// </summary>
    public class TodoListDetailedDto : TodoListDto
    {
        public List<TodoItemDto> Items { get; set; }
    }
}
