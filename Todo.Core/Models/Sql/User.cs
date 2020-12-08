using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Todo.Core.Models.Common;

namespace Todo.Core.Models.Sql
{
    /// <summary>
    /// Entity to store user information
    /// </summary>
    public class User : UserBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }        
    }
}
