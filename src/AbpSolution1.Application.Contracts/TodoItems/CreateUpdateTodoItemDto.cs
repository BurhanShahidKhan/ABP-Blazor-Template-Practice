using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AbpSolution1.TodoItems
{
    public class CreateUpdateTodoItemDto
    {
        [Required]
        [StringLength(256)] // Adjust length as needed for your requirements
        public string Text { get; set; } = string.Empty;
    }
}
