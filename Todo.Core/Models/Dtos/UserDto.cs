using System.ComponentModel.DataAnnotations;
using Todo.Core.Models.Common;

namespace Todo.Core.Models.Dtos
{
    public class UserDto : UserBase
    {
        [Required]
        public string Password { get; set; }
    }
}
