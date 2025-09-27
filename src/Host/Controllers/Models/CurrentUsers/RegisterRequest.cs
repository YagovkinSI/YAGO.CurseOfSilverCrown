using System.ComponentModel.DataAnnotations;
using YAGO.World.Host.Controllers.Models.CurrentUsers.Attributes;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers
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