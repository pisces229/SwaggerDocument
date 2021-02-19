using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace SwaggerDocument.Models
{
    public class PostJsonResponseValueInputModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public DateTime BornDate { get; set; }
        public string Address { get; set; }
    }
}
