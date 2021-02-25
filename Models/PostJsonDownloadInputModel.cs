using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace SwaggerDocument.Models
{
    public class PostJsonDownloadInputModel
    {
        [BindRequired]
        public string Path { get; set; }
        [BindRequired]
        public string Name { get; set; }
    }
}
