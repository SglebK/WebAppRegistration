using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppRegistration.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ім'я не може бути порожнім!")]
        [RegularExpression(@"^[A-ZА-ЯІЇЄ][a-zа-яіїє'-]+$",
            ErrorMessage = "Ім'я повинно починатися з великої літери і містити тільки літери, апостроф або дефіс.")]
        [Display(Name = "Ім'я")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email не може бути порожнім!")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Логін не може бути порожнім!")]
        [RegularExpression(@"^[^:]*$", ErrorMessage = @"У логіні не допускається "":"" (двокрапка)!")]
        [Display(Name = "Логін")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Пароль не може бути порожнім!")]
        [StringLength(100, ErrorMessage = "{0} повинен бути довжиною мінімум {2} символів.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Пароль повинен містити велику і малу літери, цифру та спеціальний символ (@$!%*?&).")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються!")]
        public string? RepeatPassword { get; set; }

        [Required(ErrorMessage = "Вкажіть дату народження")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата народження")]
        public DateTime? Birthday { get; set; }
    }
}