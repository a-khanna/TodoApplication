﻿using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Dtos
{
    public class UpdateTodoListDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
    }
}
