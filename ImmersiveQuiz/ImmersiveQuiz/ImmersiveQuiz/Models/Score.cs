using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class Score
    {
        public int ScoreId { get; set; }
        public string StudentId { get; set; }

        public int CourseId { get; set; }

        public decimal TimeScore { get; set; }
        public decimal PointScore { get; set; }

        public decimal TotalScore { get { return TimeScore + PointScore; } }
    }
}
