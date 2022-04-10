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
    public class ScoresController : Controller
    {
        private readonly ScoreContext _scoreContext;
        private readonly CourseContext _courseContext;

        public ScoresController(ScoreContext context, CourseContext courseContext)
        {
            _scoreContext = context;
            _courseContext = courseContext;
        }

        // GET: Scores
        public async Task<IActionResult> Index(int? id) // Passing in courseId
        {
            var scores = from score in _scoreContext.Score
                         select score;

            if (!id.HasValue)
            {
                scores = scores.Where(score => score.CourseId == id);
            }

            var courses = from course in _courseContext.Course
                         select course;



            CourseScoreViewModel vm = new CourseScoreViewModel
            {
                Scores = await scores.ToListAsync(),
                Course = await courses.FirstOrDefaultAsync()
            };

            return View(vm);
        }

        // GET: Scores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _scoreContext.Score
                .FirstOrDefaultAsync(m => m.ScoreId == id);
            if (score == null)
            {
                return NotFound();
            }

            return View(score);
        }

        // GET: Scores/Create
        public IActionResult Create(int? id) // passing in courseId
        {

            Score score = new Score()
            {
                CourseId = id ?? 0
            };

            return View(score);
        }

        // POST: Scores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScoreId,StudentId,CourseId,TimeScore,PointScore")] Score score)
        {
            if (ModelState.IsValid)
            {
                _scoreContext.Add(score);
                await _scoreContext.SaveChangesAsync();
                return RedirectToAction("Index", "Scores", new { id = score.CourseId });
            }
            return View(score);
        }

        // GET: Scores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _scoreContext.Score.FindAsync(id);
            if (score == null)
            {
                return NotFound();
            }
            return View(score);
        }

        // POST: Scores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScoreId,StudentId,CourseId,TimeScore,PointScore")] Score score)
        {
            if (id != score.ScoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _scoreContext.Update(score);
                    await _scoreContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoreExists(score.ScoreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Scores", new { id = score.CourseId });
            }
            return View(score);
        }

        // GET: Scores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _scoreContext.Score
                .FirstOrDefaultAsync(m => m.ScoreId == id);
            if (score == null)
            {
                return NotFound();
            }

            return View(score);
        }

        // POST: Scores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var score = await _scoreContext.Score.FindAsync(id);
            _scoreContext.Score.Remove(score);
            await _scoreContext.SaveChangesAsync();
            return RedirectToAction("Index", "Scores", new { id = score.CourseId });
        }

        private bool ScoreExists(int id)
        {
            return _scoreContext.Score.Any(e => e.ScoreId == id);
        }
    }
}
