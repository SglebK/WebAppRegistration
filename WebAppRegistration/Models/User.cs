using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppRegistration.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}