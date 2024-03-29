﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        [DisplayName("Question")]
        [Required(ErrorMessage = "Please enter question content")]
        public string Content { get; set; }

        public int LocationId { get; set; }
        
        [NotMapped]
        public Location Location { get; set; }

        [NotMapped]
        public List<Answer> Answers { get; set; }
    }
}
