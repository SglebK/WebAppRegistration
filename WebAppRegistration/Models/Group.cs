using System.ComponentModel.DataAnnotations;

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
        public string Description { get; set; }
    }
}