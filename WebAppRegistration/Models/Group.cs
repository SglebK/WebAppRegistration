using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppRegistration.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва групи не може бути порожньою.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Назва групи повинна містити від 3 до 100 символів.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Опис не може перевищувати 500 символів.")]
        public string? Description { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Group? Parent { get; set; }

        public ICollection<Group>? Subgroups { get; set; }

        public string? ImageUrl { get; set; }
        public string? Slug { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}