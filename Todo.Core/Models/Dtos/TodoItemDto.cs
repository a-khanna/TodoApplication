using System.Collections.Generic;

namespace Todo.Core.Models.Dtos
{
    public class TodoItemDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public List<string> Labels { get; set; }
    }
}
