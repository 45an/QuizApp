using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Server.Data;
using QuizApp.Server.Models;
using QuizApp.Server.Models.ViewModels;

namespace QuizApp.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuizController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("getallquizzes")]
        public List<QuizView> GetAllQuizzes()
        {
            var quizzes = _context.Quizzes.ToList();

            List<QuizView> quizzesView = new List<QuizView>();

            foreach (var quiz in quizzes)
            {
                quizzesView.Add(QuizConverter.ConvertQuiz(quiz));
            }

            return quizzesView;
        }

        [HttpGet("getquiz/{title}")]
        public ActionResult GetQuiz(string title)
        {
            var quiz = _context
                .Quizzes.Include(q => q.Questions)
                .ThenInclude(q => q.MocksAnswer)
                .Where(t => t.Title == title)
                .FirstOrDefault();

            if (quiz == null)
            {
                return NotFound(new { Message = "Quiz could not be found." }); // Returnera en NotFound HTTP-statuskod om quiz inte hittades med meddelandet "Quiz could not be found."
            }

            //var quizView = QuizConverter.ConvertQuiz(quiz);
            return Ok(quiz);
        }

        [HttpPost("addquiz")]
        public async Task<ActionResult> AddQuiz([FromBody] Quiz quizModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Create a new Quiz object and add it to the database
            var quizToAdd = new Models.Quiz
            {
                UserId = userId,
                Title = quizModel.Title,
                DateCreated = DateTime.Now,
                MaxScore = quizModel.Questions.Count * 100, // Assume each question is worth 100 points since there's no point for each question
                GamesPlayed = 0,
            };
            _context.Quizzes.Add(quizToAdd);
            await _context.SaveChangesAsync();

            // Loop through each question in the quiz and add them to the database
            foreach (var questionModel in quizModel.Questions)
            {
                var questionToAdd = new Question
                {
                    QuizId = quizToAdd.Id,
                    Questions = questionModel.Questions,
                    Answer = questionModel.Answer,
                    Media = questionModel.Media,
                    Time = questionModel.Time,
                    MultipleChoice = questionModel.MultipleChoice,
                    MocksAnswer = new List<Mock>()
                };
                _context.Questions.Add(questionToAdd);
                await _context.SaveChangesAsync();

                // Add mock answers if it's a multiple choice question
                if (questionModel.MultipleChoice)
                {
                    foreach (var mockAnswerModel in questionModel.MocksAnswer)
                    {
                        var mockToAdd = new Mock
                        {
                            QuestionId = questionToAdd.Id,
                            MockAnswer = mockAnswerModel.MockAnswer
                        };
                        _context.Mocks.Add(mockToAdd);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            var quizView = QuizConverter.ConvertQuiz(quizModel);

            return Ok(quizView);
        }
    }
}
