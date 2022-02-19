using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImmersiveQuiz.Data;
using ImmersiveQuiz.Models;

namespace ImmersiveQuiz.Controllers
{
    public class AnswersController : Controller
    {
        
        private readonly AnswerContext _answerContext;
        private readonly QuestionContext _questionContext;
        private readonly LocationContext _locationContext;



        public AnswersController(AnswerContext context, QuestionContext questionContext, LocationContext locationContext)
        {
            _answerContext = context;
            _questionContext = questionContext;
            _locationContext = locationContext;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            return View(await _answerContext.Answer.ToListAsync());
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _answerContext.Answer
                .FirstOrDefaultAsync(m => m.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }

            answer.Question = _questionContext.Question.Find(answer.QuestionId);

            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create(int? id) // passing in questionId
        {
            Answer answer = new Answer();

            if (id.HasValue)
            {
                answer.QuestionId = id.Value;
            }

            var question = _questionContext.Question.Find(answer.QuestionId);
            if (question == null)
            {
                return NotFound();
            }

            answer.Question = question;

            var location = _locationContext.Location.Find(answer.Question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            answer.Question.Location = location;

            return View(answer);
        }

        // POST: Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Answer answer, int id) // passing in question Id
        {
            answer.QuestionId = id;
            if (ModelState.IsValid)
            {
                _answerContext.Add(answer);
                await _answerContext.SaveChangesAsync();

                var question = _questionContext.Question.Find(id);

                var location = _locationContext.Location.Find(question.LocationId);
                if (location == null)
                {
                    return NotFound();
                }

                return RedirectToAction("Details", "Courses", new { id = location.CourseId });
            }
            return View(answer);
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _answerContext.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            var question = await _questionContext.Question.FindAsync(answer.QuestionId);
            if (question == null)
            {
                return NotFound();
            }

            answer.Question = question;

            var location = _locationContext.Location.Find(answer.Question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            answer.Question.Location = location;

            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnswerId,IsCorrect,Content")] Answer answer)
        {
            if (id != answer.AnswerId)
            {
                return NotFound();
            }
            var ans = await _answerContext.Answer.FindAsync(answer.AnswerId);

            ans.IsCorrect = answer.IsCorrect;
            ans.Content = answer.Content;

            if (ModelState.IsValid)
            {
                try
                {
                    _answerContext.Update(ans);
                    await _answerContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(ans.AnswerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                Question question = _questionContext.Question.First(q => q.QuestionId == ans.QuestionId);

                var location = _locationContext.Location.Find(question.LocationId);
                if (location == null)
                {
                    return NotFound();
                }

                return RedirectToAction("Details", "Courses", new { id = location.CourseId });
            }
            return View(answer);
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _answerContext.Answer
                .FirstOrDefaultAsync(m => m.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }

            var question = await _questionContext.Question.FindAsync(answer.QuestionId);
            if (question == null)
            {
                return NotFound();
            }

            answer.Question = question;

            var location = _locationContext.Location.Find(answer.Question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            answer.Question.Location = location;

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _answerContext.Answer.FindAsync(id);
            _answerContext.Answer.Remove(answer);
            await _answerContext.SaveChangesAsync();
            Question question = _questionContext.Question.First(q => q.QuestionId == answer.QuestionId);

            var location = _locationContext.Location.Find(question.LocationId);
            if (location == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Courses", new { id = location.CourseId });
        }

        private bool AnswerExists(int id)
        {
            return _answerContext.Answer.Any(e => e.AnswerId == id);
        }
    }
}
