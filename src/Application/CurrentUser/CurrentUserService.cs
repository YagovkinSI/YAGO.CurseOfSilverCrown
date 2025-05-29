using System.Security.Claims;
using System.Threading.Tasks;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.CurrentUser
{
    public interface ICurrentUserService
    {
        Task<User> Get(ClaimsPrincipal userClaimsPrincipal);

        Task<bool> IsAdmin(string userId);
    }
}
