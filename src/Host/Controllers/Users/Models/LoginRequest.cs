using YAGO.World.Host.Controllers.Users.Attributes;

namespace YAGO.World.Host.Controllers.Users.Models
{
    public record LoginRequest(
        [LoginValidation] string UserName,
        [PasswordValidation] string Password);
}
