using System.ComponentModel.DataAnnotations;

namespace YSI.CurseOfSilverCrown.Web.ApiModels
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Требуется логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Требуется пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Требуется повторить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}
