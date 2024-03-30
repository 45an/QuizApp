using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using QuizApp.Server.Data;
using QuizApp.Server.Models;
using QuizApp.Server.Models.ViewModels;

namespace QuizApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MediaController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostingEnvironment
        )
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

        [HttpPost("addMedia")]
        public async Task<IActionResult> UploadMediaAsync([FromForm] IFormFile file)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
                // Calculate the hash of the uploaded file
                var contentBytes = new byte[file.Length];
                using (var stream = file.OpenReadStream())
                {
                    await stream.ReadAsync(contentBytes, 0, contentBytes.Length);
                }
                var hash = CalculateHash(contentBytes);

                // Check if the same hash already exists in the database
                var existingMedia = _context.Media.FirstOrDefault(m => m.Hash == hash);
                if (existingMedia != null)
                {
                    return Ok(existingMedia);
                }
                existingMedia = _context.Media.FirstOrDefault(m => m.FileBytes == contentBytes);
                if (existingMedia != null)
                {
                    return Ok(existingMedia);
                }

                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var relativeFilePath = Path.Combine("uploads", uniqueFileName); // Path relative to wwwroot
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, relativeFilePath);

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                var fullPath = $"{baseUrl}/{relativeFilePath.Replace("\\", "/")}";

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var newMedia = new Media
                {
                    Guid = Guid.NewGuid(),
                    Hash = hash,
                    Path = fullPath,
                    ContentType = file.ContentType,
                    //FileBytes = contentBytes,
                    UserId = user
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

        // Method to calculate the hash of a byte array
        private string CalculateHash(byte[] bytes)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
