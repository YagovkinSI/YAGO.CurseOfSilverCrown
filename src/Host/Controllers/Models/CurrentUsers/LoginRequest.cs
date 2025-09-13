using YAGO.World.Host.Controllers.Models.CurrentUsers.Attributes;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers
{
    public class LoginRequest
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [LoginValidation]
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [PasswordValidation]
        public string Password { get; set; }
    }
}