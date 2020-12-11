using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Dtos
{
    public class UpdateLabelDto
    {
        [Required]
        public int ParentId { get; set; }

        [Required]
        [StringLength(100)]
        public string CurrentValue { get; set; }

        [Required]
        [StringLength(100)]
        public string NewValue { get; set; }
    }
}
