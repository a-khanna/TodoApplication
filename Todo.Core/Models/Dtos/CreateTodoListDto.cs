using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Dtos
{
    public class CreateTodoListDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
    }
}
