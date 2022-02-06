using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class SubmitScore
    {
        public string Name { get; set; }

        public int CourseId { get; set; }

        public float TimeScore { get; set; }
        public float PointScore { get; set; }

        public float TotalScore { get { return TimeScore + PointScore; } }
    }
}
