using System.ComponentModel.DataAnnotations;

namespace WebAppRegistration.Models
{
    public class RequestLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Path { get; set; }

        public string? UserLogin { get; set; }

        [Required]
        public int StatusCode { get; set; }
    }
}