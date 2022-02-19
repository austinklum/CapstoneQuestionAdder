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
    public class QuestionListViewComponent : ViewComponent
    {
        private readonly QuestionContext _questionContext;
        private readonly LocationContext _locationContext;
        private readonly AnswerContext _answerContext;
        
        public QuestionListViewComponent(QuestionContext questionContext, LocationContext locationContext, AnswerContext answerContext)
        {
            _questionContext = questionContext;
            _locationContext = locationContext;
            _answerContext = answerContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(string locationId, string search)
        {
            IQueryable<int> locationQuery = from location in _locationContext.Location
                                            orderby location.Name
                                            select location.LocationId;

            var questions = from m in _questionContext.Question
                            select m;

            var answers = from m in _answerContext.Answer
                            select m;

            if (!string.IsNullOrEmpty(search))
            {
                questions = questions.Where(s => s.Content.Contains(search));
            }

            if (!string.IsNullOrEmpty(locationId))
            {
                questions = questions.Where(x => x.LocationId == int.Parse(locationId));
            }

            foreach(var question in questions)
            {
                question.Answers = await answers.Where(x => x.QuestionId == question.QuestionId).ToListAsync();
            }

            var vm = new QuestionLocationViewModel
            {
                LocationId = locationId,
                Search = search,
                Locations = new SelectList(await locationQuery.Distinct().ToListAsync()),
                Questions = await questions.ToListAsync()
            };

            return View(vm);
        }
    }
}
