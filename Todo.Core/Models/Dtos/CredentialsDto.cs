using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Dtos
{
    public class CredentialsDto
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
