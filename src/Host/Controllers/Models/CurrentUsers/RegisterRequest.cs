using System.ComponentModel.DataAnnotations;
using YAGO.World.Host.Controllers.Models.CurrentUsers.Attributes;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers
{
    public class RegisterRequest
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [LoginValidation]
        public required string UserName { get; set; }

        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        [EmailValidation]
        public string? Email { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [PasswordValidation]
        public required string Password { get; set; }

        /// <summary>
        /// Подтверждение пароля пользователя
        /// </summary>
        [Required(ErrorMessage = "Требуется повторить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public required string PasswordConfirm { get; set; }
    }
}