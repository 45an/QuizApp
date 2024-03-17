using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using QuizApp.Server.Data;
using QuizApp.Server.Models;
using QuizApp.Server.Models.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MediaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetMedia(Guid guid)
        {
            try
            {
                var media = await _context.Media.FindAsync(guid);

                if (media == null)
                    return NotFound(); // Om media inte hittades

                // Mapa till en ViewModel för att styra utdata
                var returnMedia = new MediaView
                {
                    ContentType = media.ContentType,
                    Path = media.Path
                };

                return Ok(returnMedia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving media: {ex.Message}"); // Returnera en 500 HTTP-statuskod om ett fel inträffade
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadMediaAsync([FromForm] IFormFile file)
        {
            // Kontrollera om filen är giltig och uppfyller kraven
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded."); // Returnera en BadRequest HTTP-statuskod om ingen fil har laddats upp  

            int maxMb = 13;
            long megaByte = 1024 * 1024;
            long maxAllowedSizeInBytes = maxMb * megaByte;

            if (file.Length > maxAllowedSizeInBytes)
                return BadRequest("File size exceeds the allowable limit."); // Returnera en BadRequest HTTP-statuskod om filen är för stor

            string[] permittedFileTypes = { ".jpg", ".jpeg", ".png", ".gif", ".mp4" };
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !permittedFileTypes.Contains(extension))
                return BadRequest($"Invalid file type. Submitted filetype: {file.ContentType}");

            try
            {

                // Tillfällig kod för att visa att uppladdningen lyckades
                var newMedia = new Media
                {
                    Guid = Guid.NewGuid(),
                    ContentType = file.ContentType,
                    Path = "YourFilePathHere", // ett annat sätt att hantera filen
                };

                _context.Media.Add(newMedia);
                _context.SaveChanges();

                return Ok(newMedia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while uploading media: {ex.Message}"); // Returnera en 500 HTTP-statuskod om ett fel inträffade  
            }
        }
    }
}
