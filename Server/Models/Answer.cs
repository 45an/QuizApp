using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Server.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("OriginalQuiz")]
        public int? OriginalQuizId { get; set; }
        public virtual Quiz? OriginalQuiz { get; set; }

        [ForeignKey("AnswerQuiz")]
        public int? AnswerQuizId { get; set; }
        public virtual Quiz? AnswerQuiz { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
