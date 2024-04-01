using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Server.Models
{
    public class Media
    {
        [Key]
        public int Id { get; set; }
        public Guid MediaGuid { get; set; }
        public string? Hash { get; set; }
        public string? Path { get; set; }
        public string? ContentType { get; set; }
        public byte[]? FileBytes { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
