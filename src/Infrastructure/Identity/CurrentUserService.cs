using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUser;
using YAGO.World.Domain.CurrentUser;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Helpers;

namespace YAGO.World.Infrastructure.Identity
{
    internal class CurrentUserService : ICurrentUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public CurrentUserService(
            UserManager<User> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Domain.Users.User?> FindCurrentUser(ClaimsPrincipal userClaimsPrincipal)
        {
            var dbUser = await _userManager.GetCurrentUser(userClaimsPrincipal, _context);
            return dbUser?.ToDomain();
        }

        public async Task<AuthorizationData> GetAuthorizationData(
            ClaimsPrincipal userClaimsPrincipal,
            CancellationToken cancellationToken)
        {
            var user = await FindCurrentUser(userClaimsPrincipal);
            return new AuthorizationData(user);
        }

        public async Task<bool> IsAdmin(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            var dbUser = await _userManager.FindByIdAsync(userId);
            return await _userManager.IsInRoleAsync(dbUser, "Admin");
        }
    }
}
