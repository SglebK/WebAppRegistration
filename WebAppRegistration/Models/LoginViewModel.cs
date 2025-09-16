using System.ComponentModel.DataAnnotations;

namespace WebAppRegistration.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Логін не може бути порожнім")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль не може бути порожнім")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}