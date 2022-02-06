using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImmersiveQuiz.Data;
using ImmersiveQuiz.Models;
using ImmersiveQuiz.Models.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImmersiveQuiz.ApiControllers
{
    [BasicAuthorization]
    [Route("ImmersiveQuizAPI")]
    [ApiController]
    public class ImmersiveQuizAPI : ControllerBase
    {
        private readonly LocationContext _locationContext;
        private readonly QuestionContext _questionContext;
        private readonly AnswerContext _answerContext;

        public ImmersiveQuizAPI(LocationContext locationContext, QuestionContext questionContext, AnswerContext answerContext)
        {
            _locationContext = locationContext;
            _questionContext = questionContext;
            _answerContext = answerContext;
        }

        // GET: api/<controller>
        [HttpGet("AllLocations")]
        public List<LocationVR> GetAllLocations()
        {
            List<LocationVR> locations = _locationContext.Location
                .Select(l => new LocationVR()
                {
                    LocationId = l.LocationId,
                    Name = l.Name,
                    ImagePath = l.ImagePath
                }).ToList() ;

            return locations;
        }

        // GET api/<controller>/5
        [HttpGet("AllQuestions")]
        public List<QuestionVR> GetAllQuestions()
        {
            List<AnswerVR> answers = _answerContext.Answer
                  .Select(a => new AnswerVR()
                  {
                      AnswerId = a.AnswerId,
                      QuestionId = a.QuestionId,
                      Content = a.Content,
                      IsCorrect = a.IsCorrect
                  }).ToList();

            List<QuestionVR> questions = _questionContext.Question
              .Select(q => new QuestionVR()
              {
                  QuestionId = q.QuestionId,
                  LocationId = q.LocationId,
                  Content = q.Content
              }).ToList();

            foreach (QuestionVR question in questions)
            {
                question.Answers = answers.Where(ans => ans.QuestionId == question.QuestionId).ToList();
            }

            return questions;
        }

        [HttpGet("LocationsQuestions/{locationId}")]
        public List<QuestionVR> GetQuestionsByLocation(int locationId)
        {
            List<AnswerVR> answers = _answerContext.Answer
                  .Select(a => new AnswerVR()
                  {
                      AnswerId = a.AnswerId,
                      QuestionId = a.QuestionId,
                      Content = a.Content,
                      IsCorrect = a.IsCorrect
                  }).ToList();

            List<QuestionVR> questions = _questionContext.Question
                .Where(q => q.LocationId == locationId)
                .Select(q => new QuestionVR()
                {
                    QuestionId = q.QuestionId,
                    LocationId = q.LocationId,
                    Content = q.Content
                }).ToList();

            foreach (QuestionVR question in questions)
            {
                question.Answers = answers.Where(ans => ans.QuestionId == question.QuestionId).ToList();
            }

            return questions;
        }

        // POST api/<controller>
        [HttpPost("SubmitScore")]
        public void Post([FromBody] SubmitScore submitScore)
        {
            if (submitScore.PointScore < 0)
            {
                throw new InvalidOperationException();
            }
        }
  
            // PUT api/<controller>/5
            [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
