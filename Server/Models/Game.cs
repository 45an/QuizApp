using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Server.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Answser")]
        public int AnswerId { get; set; }
        public Answer? Answer { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        [Required]
        public int Score { get; set; }
    }
}
