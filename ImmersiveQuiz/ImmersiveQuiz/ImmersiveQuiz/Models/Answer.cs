using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public string Content { get; set; }

        public int QuestionId { get; set; }
        
        [DisplayName("Correct Answer")]
        public bool IsCorrect { get; set; }
    }
}
