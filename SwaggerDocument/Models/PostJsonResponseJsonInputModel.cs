using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwaggerDocument.Models
{
    public class PostJsonResponseJsonInputModel
    {
        [Required]
        public DateTime UploadDate { get; set; }
        [Required]
        public List<PostJsonResponseJsonDataInputModel> Datas { get; set; }
    }
    public class PostJsonResponseJsonDataInputModel
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
