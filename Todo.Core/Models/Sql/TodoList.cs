using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Core.Models.Sql
{
    /// <summary>
    /// Entity to store Todo list
    /// </summary>
    public class TodoList : Metadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public virtual ICollection<TodoItem> Items { get; set; }

        public virtual ICollection<Label> Labels { get; set; }
    }
}
