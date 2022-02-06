using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class CourseLocationViewModel
    {
            public List<Location> Locations { get; set; }

            public SelectList Courses { get; set; }

            public string SearchCourseId { get; set; }

            public string SearchCourseContent { get; set; }
    }
}
