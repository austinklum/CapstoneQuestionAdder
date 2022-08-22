using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImmersiveQuiz.Data;
using ImmersiveQuiz.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.AspNetCore.Authorization;

namespace ImmersiveQuiz.Controllers
{
    [Authorize(Roles = "Verified")]
    public class QuestionsController : Controller
    {
        private readonly QuestionContext _questionContext;
        private readonly LocationContext _locationContext;
        private readonly AnswerContext _answerContext;

        public QuestionsController(QuestionContext context, LocationContext locationContext, AnswerContext answerContext)
        {
            _questionContext = context;
            _locationContext = locationContext;
            _answerContext = answerContext;
        }

        // GET: Questions
        public async Task<IActionResult> Index(string locationId, string search)
        {
            // Use LINQ to get list of genres.
            IQueryable<int> locationQuery = from location in _locationContext.Location
                                               orderby location.Name
                                               select location.LocationId;

            var questions = from m in _questionContext.Question
                            select m;

            if (!string.IsNullOrEmpty(search))
            {
                questions = questions.Where(s => s.Content.Contains(search));
            }

            if (!string.IsNullOrEmpty(locationId))
            {
                questions = questions.Where(x => x.LocationId == int.Parse(locationId));
            }

            var vm = new QuestionLocationViewModel
            {
                Locations = new SelectList(await locationQuery.Distinct().ToListAsync()),
                Questions = await questions.ToListAsync()
            };

            return View(vm);
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _questionContext.Question
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            var location = await _locationContext.Location.FindAsync(question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            question.Location = location;

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create(int? id) //passing in location id
        {
            Question question = new Question();

            if (id.HasValue)
            {
                question.LocationId = id.Value;
            }

            var location = _locationContext.Location.Find(question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            question.Location = location;

            return View(question);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Question question, int id) // passing in location Id
        {
            question.LocationId = id;
            if (ModelState.IsValid)
            {
                _questionContext.Add(question);
                await _questionContext.SaveChangesAsync();

                var location = _locationContext.Location.Find(question.LocationId);
                if (location == null)
                {
                    return NotFound();
                }

                return RedirectToAction("Details", "Courses", new { id = location.CourseId });
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _questionContext.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            var location = await _locationContext.Location.FindAsync(question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            question.Location = location;

            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,Content,LocationId")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            var ques = await _questionContext.Question.FindAsync(question.QuestionId);

            ques.Content = question.Content;

            if (ModelState.IsValid)
            {
                try
                {
                    _questionContext.Update(ques);
                    await _questionContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var location = _locationContext.Location.Find(ques.LocationId);
                if (location == null)
                {
                    return NotFound();
                }

                return RedirectToAction("Details", "Courses", new { id = location.CourseId });
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _questionContext.Question
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            var location = await _locationContext.Location.FindAsync(question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            question.Location = location;

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _questionContext.Question.FindAsync(id);

            var answersToQuestion = _answerContext.Answer.Where(ans => ans.QuestionId == question.QuestionId);
            _answerContext.Answer.RemoveRange(answersToQuestion);
            await _answerContext.SaveChangesAsync();

            _questionContext.Remove(question);
            await _questionContext.SaveChangesAsync();

            var location = _locationContext.Location.Find(question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Courses", new { id = location.CourseId });
        }

        private bool QuestionExists(int id)
        {
            return _questionContext.Question.Any(e => e.QuestionId == id);
        }
    }
}
