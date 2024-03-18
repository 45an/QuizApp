using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using QuizApp.Server.Data;
using QuizApp.Server.Models;

namespace QuizApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet("mygames")]
        public async Task<IActionResult> GetUsersGame()
        {
            var user = await _userManager.GetUserAsync(User);
            var games = _context.Games.Where(g => g.UserId == user.Id).ToList();
            return Ok(games);
        }

        [HttpGet("myquizzes")]
        public async Task<IActionResult> GetCreatedGames()
        {
            try
            {
                var userId = _userManager.GetUserId(User); // Hämta användarens ID
                var userQuizzes = await _context.Quizzes
                    .Where(q => q.UserId == userId) // Filtrera quiz som är skapade av användaren
                    .ToListAsync();

                return Ok(userQuizzes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user created games: {ex.Message}"); // Returnera en 500 HTTP-statuskod om ett fel inträffade    
            }
        }

    }
}
