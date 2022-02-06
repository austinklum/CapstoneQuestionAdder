using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class Course
    {
            public int CourseId { get; set; }

            [DisplayName("Name")]
            public string Name { get; set; }

    }
}
