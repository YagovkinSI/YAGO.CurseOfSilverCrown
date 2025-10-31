using System.ComponentModel.DataAnnotations;
using YAGO.World.Host.Controllers.Users.Attributes;

namespace YAGO.World.Host.Controllers.Users
{
    public record RegisterRequest(
        [LoginValidation] string UserName,
        [EmailValidation] string? Email,
        [PasswordValidation] string Password
    )
    {
        [Required(ErrorMessage = "Требуется повторить пароль")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public required string PasswordConfirm { get; init; }
    }
}