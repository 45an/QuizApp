using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Server.Models
{
    public class Mock
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }
        public virtual Question? Question { get; set; }

        public string? MockAnswer { get; set; }
    }
}
