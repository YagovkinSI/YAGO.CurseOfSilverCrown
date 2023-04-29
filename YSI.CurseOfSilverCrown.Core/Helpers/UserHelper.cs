using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.APIModels;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Users;

namespace YSI.CurseOfSilverCrown.Core.Helpers
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

        public static async Task<UserPrivate> GetUserPrivateAsync(this DbSet<User> users, string userId)
        {
            if (users == null) 
                return null;

            var user = await users.FindAsync(userId);
            if (user == null)
                return null;

            return new UserPrivate
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }
    }
}
