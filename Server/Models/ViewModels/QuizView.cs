using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizApp.Server.Models;

namespace QuizApp.Server.Models.ViewModels
{
    public class QuizView
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string? Title { get; set; }
        public int GamesPlayed { get; set; }
        public DateTime DateCreated { get; set; }
        public int MaxScore { get; set; }
        public List<QuestionView>? Questions { get; set; } = new List<QuestionView>();
    }
}
