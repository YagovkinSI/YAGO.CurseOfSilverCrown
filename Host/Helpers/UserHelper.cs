using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Users;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers
{
    public static class UserHelper
    {
        public static async Task<User> GetCurrentUser(this UserManager<User> userManager, ClaimsPrincipal claimsPrincipal, ApplicationDbContext context)
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);
            if (user != null)
            {
                user.LastActivityTime = DateTime.UtcNow;
                _ = context.Update(user);
                _ = await context.SaveChangesAsync();
            }
            return user;
        }

        public static async Task<User> AccessСheckAndGetCurrentUser(ApplicationDbContext _context, UserManager<User> _userManager,
            ClaimsPrincipal userClaimsPrincipal, int? domainId)
        {
            if (domainId == null)
                return null;
            var domain = _context.Domains.Find(domainId.Value);

            var currentUser = await _userManager.GetCurrentUser(userClaimsPrincipal, _context);

            return domain.UserId == currentUser.Id
                ? currentUser
                : null;
        }
    }
}
