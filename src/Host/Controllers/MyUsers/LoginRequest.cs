using YAGO.World.Host.Controllers.MyUsers.Attributes;

namespace YAGO.World.Host.Controllers.MyUsers
{
    public record LoginRequest(
        [LoginValidation] string UserName,
        [PasswordValidation] string Password);
}
