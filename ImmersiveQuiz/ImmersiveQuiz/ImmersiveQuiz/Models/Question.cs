using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        [DisplayName("Question")]
        public string Content { get; set; }

        public int CorrectAnswerId { get; set; }

        public int LocationId { get; set; }
    }
}
