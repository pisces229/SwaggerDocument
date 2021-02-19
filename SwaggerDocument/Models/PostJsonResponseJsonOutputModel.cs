using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace SwaggerDocument.Models
{
    public class PostJsonResponseJsonOutputModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime BornDate { get; set; }
        public string Address { get; set; }
    }
}
