using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class QuestionVR
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int LocationId { get; set; }

        public List<AnswerVR> Answers { get; set; }
    }
}
