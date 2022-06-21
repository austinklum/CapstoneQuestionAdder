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
        private readonly CourseContext _courseContext;
        private readonly LocationContext _locationContext;
        private readonly QuestionContext _questionContext;
        private readonly AnswerContext _answerContext;
        private readonly ScoreContext _scoreContext;

        public ImmersiveQuizAPI(CourseContext courseContext, LocationContext locationContext, QuestionContext questionContext, AnswerContext answerContext, ScoreContext scoreContext)
        {
            _courseContext = courseContext;
            _locationContext = locationContext;
            _questionContext = questionContext;
            _answerContext = answerContext;
            _scoreContext = scoreContext;
        }

        public ImmersiveQuizAPI()
        {

        }

        [HttpGet("AllCourses")]
        public List<CourseVR> GetAllCourses()
        {
            List<CourseVR> courses = _courseContext.Course
                .Select(l => new CourseVR()
                {
                  CourseId = l.CourseId,
                  Name = l.Name
                }).ToList();

            return courses;
        }

        [HttpGet("LocationsByCourseId/{courseId}")]
        public List<LocationVR> GetLocationsByCourseId([FromRoute] int courseId)
        {
            List<LocationVR> locations = _locationContext.Location
                .Where(l => l.CourseId == courseId)
                .Select(l => new LocationVR()
                {
                    LocationId = l.LocationId,
                    Name = l.Name,
                    ImagePath = l.ImagePath
                }).ToList();

            return locations;
        }

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
        public async Task<IActionResult> SubmitScore([FromBody] Score submitScore)
        {
            if (submitScore.PointScore < 0 || submitScore.TimeScore < 0)
            {
                return new BadRequestObjectResult("Invalid score");
            }
            _scoreContext.Add(submitScore);
            await _scoreContext.SaveChangesAsync();
            return new OkResult();
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
