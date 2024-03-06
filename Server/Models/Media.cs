using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Server.Models
{
    public class Media
    {
        [Key]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? ContentType { get; set; }
        public string? Path { get; set; }
        public byte[]? FileBytes { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
