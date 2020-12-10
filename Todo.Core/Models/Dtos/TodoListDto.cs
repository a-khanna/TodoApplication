using System.Collections.Generic;

namespace Todo.Core.Models.Dtos
{
    /// <summary>
    /// Dto for todo list
    /// </summary>
    public class TodoListDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Todo list name
        /// </summary>
        /// <example>Shopping list</example>
        public string Name { get; set; }

        public List<string> Labels { get; set; }
    }
}
