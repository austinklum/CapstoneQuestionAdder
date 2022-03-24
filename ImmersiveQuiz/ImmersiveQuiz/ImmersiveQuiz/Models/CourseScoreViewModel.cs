using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class CourseScoreViewModel
    {
        public List<Score> Scores { get; set; }

        public Course Course { get; set; }
    }
}
