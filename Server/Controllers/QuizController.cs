using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public IActionResult GetAllQuizzes()
        {
            try
            {
                var quizzes = _context.Quizzes?.Include(q => q.Media).ToList();

                List<QuizView> quizzesView = new List<QuizView>();

                foreach (var quiz in quizzes)
                {
                    quizzesView.Add(QuizConverter.Convert(quiz));
                }

                return Ok(quizzesView);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    $"An error occurred while retrieving all quizzes: {ex.Message}"
                ); // Returnera en 500 HTTP-statuskod om ett fel inträffade
            }
        }

        [HttpGet("getquiz/{quizId}")]
        public ActionResult GetQuizById(int quizId)
        {
            var quiz = _context
                .Quizzes.Include(q => q.Media != null ? q.Media : null)
                .Include(q => q.Questions != null ? q.Questions : null)
                .ThenInclude(q => q.Media != null ? q.Media : null)
                .Include(q => q.Questions != null ? q.Questions : null)
                .ThenInclude(q => q.MocksAnswers != null ? q.MocksAnswers : null)
                .Where(t => t.Id == quizId)
                .FirstOrDefault();

            if (quiz == null)
            {
                return NotFound(new { Message = "Quiz could not be found." });
            }

            var quizView = QuizConverter.Convert(quiz);
            return Ok(quizView);
        }

        [HttpPost("addquiz")]
        public async Task<ActionResult> AddQuiz([FromBody] Quiz quizModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            try
            {
                // Create a new Quiz object and add it to the database
                var quizToAdd = new Quiz
                {
                    UserId = userId,
                    UserName = user.UserName,
                    Title = quizModel.Title,
                    //Media = quizModel.Media,
                    DateCreated = DateTime.UtcNow,
                    MaxScore = quizModel.Questions.Count * 100, // Assuming each question is worth 100 points
                    GamesPlayed = 0,
                    Questions = new List<Question>() // Initialize the Questions collection
                };

                if (quizModel.Media != null && quizModel.Media.Hash != null)
                {
                    // Fetch media based on the provided Hash
                    var media = _context.Media?.FirstOrDefault(m => m.Hash == quizModel.Media.Hash);
                    // Associate media with the quiz
                    quizToAdd.Media = media;
                }

                foreach (var questionModel in quizModel.Questions)
                {
                    var questionToAdd = new Question
                    {
                        Questions = questionModel.Questions,
                        Answer = questionModel.Answer,
                        //Media = null,
                        MultipleChoice = questionModel.MultipleChoice,
                        QuizId = quizToAdd.Id, // Set QuizId for the question
                        MocksAnswers = new List<Mock>()
                    };

                    if (questionModel.Media != null && questionModel.Media.Hash != null)
                    {
                        // Fetch media based on the provided Hash
                        var mediaQuestion = _context.Media?.FirstOrDefault(m =>
                            m.Hash == questionModel.Media.Hash
                        );
                        // Associate media with the question
                        questionToAdd.Media = mediaQuestion;
                    }

                    // Add mock answers if it's a multiple choice question
                    if (questionModel.MultipleChoice)
                    {
                        foreach (var mockAnswerModel in questionModel.MocksAnswers)
                        {
                            var mockToAdd = new Mock
                            {
                                QuestionId = questionModel.Id,
                                MockAnswer = mockAnswerModel.MockAnswer
                            };
                            questionToAdd.MocksAnswers.Add(mockToAdd);
                        }
                    }

                    quizToAdd.Questions.Add(questionToAdd);
                }

                _context.Quizzes?.Add(quizToAdd);
                await _context.SaveChangesAsync();

                var quizView = QuizConverter.Convert(quizModel);

                return Ok(quizView);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"An error occurred while adding quiz: {ex.Message}");
                return StatusCode(
                    500,
                    "An error occurred while adding quiz. Please try again later."
                );
            }
        }
    }
}
