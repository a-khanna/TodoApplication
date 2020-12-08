using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Core.Models.Sql
{
    /// <summary>
    /// Entity to store Todo item
    /// </summary>
    public class TodoItem : Metadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<Label> Labels { get; set; }
    }
}
