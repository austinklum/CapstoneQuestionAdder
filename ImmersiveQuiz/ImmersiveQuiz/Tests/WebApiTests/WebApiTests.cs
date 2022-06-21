using ImmersiveQuiz.ApiControllers;
using ImmersiveQuiz.Data;
using ImmersiveQuiz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.WebApiTests
{
    public class WebApiTests
    {
        private readonly ImmersiveQuizAPI _webApi;
        private readonly Mock<CourseContext> _mockCourseContext;
        private readonly Mock<LocationContext> _mockLocationContext;
        private readonly Mock<QuestionContext> _mockQuestionContext;
        private readonly Mock<AnswerContext> _mockAnswerContext;
        private readonly Mock<ScoreContext> _mockScoreContext;
        private readonly Mock<DbContextOptionsBuilder> _mockOptions;

        public WebApiTests()
        {
            _mockOptions = new Mock<DbContextOptionsBuilder>();
            _mockCourseContext = new Mock<CourseContext>(_mockOptions.Object);
            _mockLocationContext = new Mock<LocationContext>(_mockOptions.Object);
            _mockQuestionContext = new Mock<QuestionContext>(_mockOptions.Object);
            _mockAnswerContext = new Mock<AnswerContext>(_mockOptions.Object);
            _mockScoreContext = new Mock<ScoreContext>(_mockOptions.Object);
            
            _webApi = new ImmersiveQuizAPI();
        }

        [Fact]
        public async Task Create_InvalidModel_ReturnsFalseAsync()
        {
            Score score = new Score()
            {
                StudentId = "1234",
                CourseId = 1,
                TimeScore = -1,
                PointScore = -1
            };
            
            var response = await _webApi.SubmitScore(score);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.IsType<string>(badRequestResult.Value);
        } 
    }
}
