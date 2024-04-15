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
                .Quizzes.Include(q => q.Media != null ? q.Media : null)
                .Where(x => x.Id == answerQuiz.Id)
                .FirstOrDefault();

            if (originalQuiz == null)
            {
                return NotFound();
            }

            var _answerQuiz = originalQuiz;
            answerQuiz.UserId = userId;
            answerQuiz.Questions = answerQuiz.Questions;

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
                //Score = request.Score,
            };

            _context.Games.Add(game);

            _answerQuiz.GamesPlayed += 1;
            originalQuiz.GamesPlayed += 1;

            _context.SaveChanges();

            return Ok();
        }
    }
}
