using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public virtual Media? Media { get; set; }

        [Required]
        public int Time { get; set; } = 0;
        public bool MultipleChoice { get; set; }
        public virtual List<Mock>? MocksAnswers { get; set; }
    }
}
