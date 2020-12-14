using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Dtos
{
    public class CreateLabelDto
    {
        /// <summary>
        /// This can be a todo list or a todo item
        /// </summary>
        [Required]
        public int ParentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Label { get; set; }
    }
}
