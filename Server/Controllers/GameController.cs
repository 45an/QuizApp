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

        public GameController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("savegame")] 
        public IActionResult SaveGame([FromBody] SaveGameRequest request)
        {
            var quiz = _context.Quizzes
             .Include(q => q.User)
             .Where(x => x.Title == request.Title)
             .FirstOrDefault();

            if (quiz == null)
            {
                return NotFound();
            }

            var game = new Game()
            {
                QuizId = quiz.Id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Score = request.Score,
            };
           
            _context.Games.Add(game);

            quiz.GamesPlayed += 1;

             _context.SaveChanges();

            return Ok();
        }



    }
}
