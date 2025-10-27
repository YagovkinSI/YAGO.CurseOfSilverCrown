using YAGO.World.Host.Controllers.Users.Attributes;

namespace YAGO.World.Host.Controllers.Users
{
    public record LoginRequest(
        [LoginValidation] string UserName,
        [PasswordValidation] string Password);
}
