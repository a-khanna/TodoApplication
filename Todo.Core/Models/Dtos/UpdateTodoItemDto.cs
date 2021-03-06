﻿using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Models.Dtos
{
    public class UpdateTodoItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
