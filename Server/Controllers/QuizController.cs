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
        public List<QuizView> GetAllQuizzes()
        {
            var quizzes = _context.Quizzes.ToList();

            List<QuizView> quizzesView = new List<QuizView>();

            foreach (var quiz in quizzes)
            {
                quizzesView.Add(QuizConverter.Convert(quiz));
            }

            return quizzesView;
        }

        [HttpGet("getquiz/{title}")]
        public ActionResult GetQuiz(string title)
        {
            var quiz = _context
                .Quizzes.Include(q => q.Questions)
                .ThenInclude(q => q.MocksAnswers)
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

            try
            {
                // Create a new Quiz object and add it to the database
                var quizToAdd = new Models.Quiz
                {
                    UserId = userId,
                    //Name = quizModel.Title, // Set Name property
                    Title = quizModel.Title,
                    DateCreated = DateTime.Now,
                    MaxScore = quizModel.Questions.Count * 100, // Assuming each question is worth 100 points
                    GamesPlayed = 0,
                    Questions = new List<Question>() // Initialize the Questions collection
                };

                foreach (var questionModel in quizModel.Questions)
                {
                    var questionToAdd = new Question
                    {
                        Questions = questionModel.Questions,
                        Answer = questionModel.Answer,
                        Media = questionModel.Media,
                        // Time = questionModel.Time,
                        MultipleChoice = questionModel.MultipleChoice,
                        QuizId = quizToAdd.Id, // Set QuizId for the question
                        MocksAnswers = new List<Mock>()
                    };

                    Guid mediaGuid = questionModel.Media.Guid;
                    // Fetch media based on the provided GUID
                    var media = await _context.Media.FindAsync(mediaGuid);
                    // Associate media with the question
                    questionToAdd.Media = new Media
                    {
                        Path = media.Path,
                        ContentType = media.ContentType
                    };

                    // Add mock answers if it's a multiple choice question
                    if (questionModel.MultipleChoice)
                    {
                        foreach (var mockAnswerModel in questionModel.MocksAnswers)
                        {
                            var mockToAdd = new Mock { MockAnswer = mockAnswerModel.MockAnswer };
                            questionToAdd.MocksAnswers.Add(mockToAdd);
                        }
                    }

                    quizToAdd.Questions.Add(questionToAdd);
                }

                _context.Quizzes.Add(quizToAdd);
                await _context.SaveChangesAsync();

                //var quizView = QuizConverter.ConvertQuiz(quizModel);

                return Ok(quizToAdd);
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
