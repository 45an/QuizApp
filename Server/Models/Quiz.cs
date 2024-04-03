using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizApp.Server.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public virtual Media? Media { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public int MaxScore { get; set; }

        [AllowNull]
        public int GamesPlayed { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public List<Question>? Questions { get; set; } = new List<Question>();
    }
}
