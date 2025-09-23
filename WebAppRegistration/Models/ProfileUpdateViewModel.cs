using System.ComponentModel.DataAnnotations;

namespace WebAppRegistration.Models
{
    public class ProfileUpdateViewModel
    {
        [Required(ErrorMessage = "Ім'я не може бути порожнім!")]
        [RegularExpression(@"^[A-ZА-ЯІЇЄ][a-zа-яіїє'-]+$",
            ErrorMessage = "Ім'я повинно починатися з великої літери і містити тільки літери, апостроф або дефіс.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email не може бути порожнім!")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        public string Email { get; set; }
    }
}