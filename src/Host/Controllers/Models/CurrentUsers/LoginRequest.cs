using YAGO.World.Host.Controllers.Models.CurrentUsers.Attributes;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers
{
    public record LoginRequest(
        [LoginValidation] string UserName,
        [PasswordValidation] string Password);
}
