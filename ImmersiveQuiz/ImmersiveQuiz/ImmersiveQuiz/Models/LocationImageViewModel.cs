using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class LocationImageViewModel
    {
        [Required(ErrorMessage = "Please enter location name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please upload an image")]
        [DisplayName("Location Image")]
        public IFormFile LocationImage { get; set; }
    }
}
