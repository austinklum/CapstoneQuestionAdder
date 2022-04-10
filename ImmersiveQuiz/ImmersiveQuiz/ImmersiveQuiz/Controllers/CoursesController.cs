using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImmersiveQuiz.Data;
using ImmersiveQuiz.Models;
using Microsoft.AspNetCore.Authorization;

namespace ImmersiveQuiz.Controllers
{
    [Authorize(Roles = "Verified")]
    public class CoursesController : Controller
    {
        private readonly CourseContext _courseContext;
        private readonly LocationContext _locationContext;
        private readonly QuestionContext _questionContext;
        private readonly AnswerContext _answerContext;

        
        public CoursesController(CourseContext context, LocationContext locationContext, QuestionContext questionContext, AnswerContext answerContext)
        {
            _courseContext = context;
            _locationContext = locationContext;
            _questionContext = questionContext;
            _answerContext = answerContext;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _courseContext.Course.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseContext.Course
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Name")] Course course)
        {
            if (ModelState.IsValid)
            {
                _courseContext.Add(course);
                await _courseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseContext.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Name")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _courseContext.Update(course);
                    await _courseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseContext.Course
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _courseContext.Course.FindAsync(id);

            var locations = _locationContext.Location.Where(l => l.CourseId == course.CourseId);

            foreach (var location in locations)
            {
                var questionsToLocations = _questionContext.Question.Where(q => q.LocationId == location.LocationId);
                foreach (var question in questionsToLocations)
                {
                    var answersToQuestion = _answerContext.Answer.Where(ans => ans.QuestionId == question.QuestionId);
                    _answerContext.Answer.RemoveRange(answersToQuestion);

                    _questionContext.Remove(question);
                }
                _locationContext.Location.Remove(location);
            }

            _courseContext.Course.Remove(course);

            await _answerContext.SaveChangesAsync();
            await _questionContext.SaveChangesAsync();
            await _locationContext.SaveChangesAsync();
            await _courseContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _courseContext.Course.Any(e => e.CourseId == id);
        }
    }
}
