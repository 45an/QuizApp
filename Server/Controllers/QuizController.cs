using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Server.Data;
using QuizApp.Server.Models;
using QuizApp.Server.Models.ViewModels;
using System.Security.Claims;

namespace QuizApp.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuizController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
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
            var quiz = _context.Quizzes.Include(q => q.Questions).ThenInclude(q => q.MocksAnswer)
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
        public async Task<ActionResult> AddQuiz([FromBody] Quiz quiz)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Skapa ett nytt Quiz-objekt och lägg till det i databasen 
            var quizToAdd = new Models.Quiz
            {
                UserId = userId,
                Title = quiz.Title,
                DateCreated = DateTime.Now,
                MaxScore = quiz.Questions.Count * 100, // Antag att varje fråga är värd 100 poäng eftersom det inte finns någon poäng för varje fråga
                GamesPlayed = 0,
            };
            _context.Quizzes.Add(quizToAdd);
            _context.SaveChanges();

            // Loopa igenom varje fråga i quiz och lägg till dem i databasen
            foreach (var question in quiz.Questions)
            {
                var questionToAdd = new Question
                {
                    QuizId = quizToAdd.Id,
                    Questions = question.Questions,
                    Answer = question.Answer,
                    Media = question.Media,
                    Time = question.Time,
                    MocksAnswer = new List<Mock>()
                };
                _context.Questions.Add(questionToAdd);
                _context.SaveChanges();

                // Loopa igenom varje MockAnswer för varje fråga och lägg till dem i databasen
                foreach (var mock in question.MocksAnswer)
                {
                    var mockToAdd = new Mock
                    {
                        QuestionId = questionToAdd.Id,
                        MockAnswer = mock.MockAnswer
                    };
                    _context.Mocks.Add(mockToAdd);
                }
            }
            _context.SaveChanges();

            var quizView = QuizConverter.ConvertQuiz(quiz);

            return Ok(quizView);
        }

    }
}


