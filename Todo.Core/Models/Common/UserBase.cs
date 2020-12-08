using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Common
{
    public class UserBase
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        [StringLength(200)]
        public string LastName { get; set; }
    }
}
