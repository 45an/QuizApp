using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Server.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        [ForeignKey("User")]    
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        [Required]
        public int Score { get; set; }

    }
}
