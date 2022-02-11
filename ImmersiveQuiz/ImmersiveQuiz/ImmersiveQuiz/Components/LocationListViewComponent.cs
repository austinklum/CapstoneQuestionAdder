using ImmersiveQuiz.Data;
using ImmersiveQuiz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Components
{
    public class LocationListViewComponent : ViewComponent
    {
        private readonly CourseContext _courseContext;
        private readonly LocationContext _locationContext;

        public LocationListViewComponent(CourseContext courseContext, LocationContext locationContext)
        {
            _courseContext = courseContext;
            _locationContext = locationContext;
        }

        public async Task<IViewComponentResult>
    InvokeAsync(string courseId, string search)
        {
            IQueryable<int>
                courseQuery = from course in _courseContext.Course
                                orderby course.Name
                                select course.CourseId;

            var locations = from m in _locationContext.Location
                            select m;

            if (!string.IsNullOrEmpty(search))
            {
                locations = locations.Where(s => s.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(courseId))
            {
                locations = locations.Where(x => x.CourseId == int.Parse(courseId));
            }

            var vm = new CourseLocationViewModel
            {
                CourseId = courseId,
                Search = search,
                Courses = new SelectList(await courseQuery.Distinct().ToListAsync()),
                Locations = await locations.ToListAsync()
            };

            return View(vm);
        }
    }
}
