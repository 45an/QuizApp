﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using QuizApp.Server.Data;
using QuizApp.Server.Models;
using QuizApp.Server.Models.ViewModels;

namespace QuizApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProfileController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostingEnvironment
        )
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("mygames")]
        public IActionResult GetUsersGames()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var games = _context.Games.Where(g => g.UserId == userId).ToList();

                List<GameView> gamesView = new List<GameView>();

                foreach (var game in games)
                {
                    gamesView.Add(GameConverter.Convert(game));
                }

                return Ok(gamesView);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    $"An error occurred while retrieving user's games: {ex.Message}"
                );
            }
        }

        [HttpGet("myquizzes")]
        public async Task<IActionResult> GetCreatedGames()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userQuizzes = await _context
                    .Quizzes.Where(q => q.UserId == userId)
                    .ToListAsync();

                return Ok(userQuizzes);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    $"An error occurred while retrieving user's quizzes: {ex.Message}"
                );
            }
        }

        [HttpGet("quizzes/{UserId}")]
        public async Task<IActionResult> GetCreatedQuizzes(string UserId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(UserId);

                if (user == null)
                {
                    return NotFound($"User with ID: {UserId} not found.");
                }

                var userQuizzes = await _context
                    .Quizzes.Where(q => q.UserId == user.Id)
                    .ToListAsync();

                return Ok(userQuizzes);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    $"An error occurred while retrieving user's quizzes: {ex.Message}"
                );
            }
        }

        [HttpGet("answers/{UserId}")]
        public async Task<IActionResult> GetUserAnswers(string UserId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(UserId);

                if (user == null)
                {
                    return NotFound($"User with ID: {UserId} not found.");
                }

                var userAnswers = await _context
                    .Answer.Where(a => a.UserId == user.Id)
                    .Include(a => a.OriginalQuiz)
                    .Include(a => a.AnswerQuiz)
                    .ToListAsync();

                var userAnswersView = new List<AnswerView>();
                foreach (var userAnswer in userAnswers)
                {
                    userAnswersView.Add(AnswerConverter.Convert(userAnswer));
                }

                return Ok(userAnswersView);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    $"An error occurred while retrieving user's answers: {ex.Message}"
                );
            }
        }
    }
}
