using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Server.Models
{
    public class Question
    {
        public int Id { get; set; }

        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        [Required]
        public string? Questions { get; set; }
        [Required]
        public string? Answer { get; set; }
        [Required]
        public string? Media { get; set; }
        [Required]
        public int Time { get; set; } = 0;

        public virtual List<Mock>? MocksAnswer { get; set; } = new List<Mock>();
    }
}
