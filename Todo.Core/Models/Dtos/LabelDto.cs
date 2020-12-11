using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Dtos
{
    public class LabelDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
