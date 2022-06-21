using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class Score
    {
        public int ScoreId { get; set; }

        [DisplayName("Student ID")]
        public string StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Range(0, double.MaxValue)]
        [DisplayName("Time Score")]
        public decimal TimeScore { get; set; }

        [Range(0, double.MaxValue)]
        [DisplayName("Point Score")]
        public decimal PointScore { get; set; }

        public decimal TotalScore { get { return TimeScore + PointScore; } }
    }
}
