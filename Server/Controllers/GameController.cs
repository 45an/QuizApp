using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using QuizApp.Server.Data;
using QuizApp.Server.Models;
using QuizApp.Server.Models.ViewModels;
using QuizApp.Server.Models.ViewModels.Requests;

namespace QuizApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GameController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IWebHostEnvironment hostingEnvironment
        )
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("savegame")]
        public async Task<IActionResult> SaveGame([FromBody] Quiz answerQuiz)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var originalQuiz = _context
                .Quizzes.Include(q => q.Media)
                .Include(q => q.Questions)
                .FirstOrDefault(x => x.Id == answerQuiz.Id);

            if (originalQuiz == null)
            {
                return NotFound();
            }

            originalQuiz.GamesPlayed += 1;

            // Detach originalQuiz from the context
            _context.Entry(originalQuiz).State = EntityState.Detached;

            var _answerQuiz = new Quiz
            {
                UserName = originalQuiz.UserName,
                Title = originalQuiz.Title,
                Media = originalQuiz.Media,
                DateCreated = originalQuiz.DateCreated,
                MaxScore = originalQuiz.MaxScore,
                UserId = userId,
            };

            foreach (var answerQuestion in answerQuiz.Questions)
            {
                _answerQuiz.Questions.Add(answerQuestion);
            }

            var answer = new Answer
            {
                OriginalQuiz = originalQuiz,
                AnswerQuiz = _answerQuiz,
                UserId = userId,
                User = user
            };

            var game = new Game()
            {
                Answer = answer,
                UserId = userId,
                User = user,
                Score = CalculateScore(originalQuiz, _answerQuiz),
            };

            _context.Quizzes?.Add(_answerQuiz);
            _context.Answer?.Add(answer);
            _context.Games?.Add(game);
            await _context.SaveChangesAsync();
            var gameView = GameConverter.Convert(game);
            return Ok(gameView);
        }

        private int CalculateScore(Quiz originalQuiz, Quiz answerQuiz)
        {
            // Check if OriginalQuiz is null
            if (
                originalQuiz == null
                || originalQuiz.Questions == null
                || answerQuiz == null
                || answerQuiz.Questions == null
            )
            {
                return 0; // No questions or answers, score is 0
            }

            int maxScore = originalQuiz.MaxScore;
            int score = 0;

            // Iterate through each question and compare answers
            foreach (var answerQuestion in answerQuiz.Questions)
            {
                var originalQuestion = originalQuiz.Questions.FirstOrDefault(q =>
                    q.Id == answerQuestion.Id
                );
                if (
                    answerQuestion != null
                    && originalQuestion.Answer.ToLower() == answerQuestion.Answer.ToLower()
                )
                {
                    // Correct answer, add 100 to the score
                    score += 100;
                }
                // Else: Incorrect answer, no points awarded for this question
            }

            return score;
        }
    }
}
