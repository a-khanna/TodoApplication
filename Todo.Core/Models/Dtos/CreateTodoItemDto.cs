using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Dtos
{
    public class CreateTodoItemDto
    {
        [Required]
        public int TodoListId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
