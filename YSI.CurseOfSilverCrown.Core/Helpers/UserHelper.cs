using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public static bool ValidDomain(ApplicationDbContext context, User user, int domainId,
            out Domain domain, out Domain userDomain)
        {
            domain = null;
            userDomain = null;

            if (user == null)
                return false;
            if (user.PersonId == null)
                return false;

            domain = context.Domains.Find(domainId);

            if (domain.PersonId == user.PersonId)
            {
                userDomain = domain;
                return true;
            }

            var suzerainId = domain.SuzerainId;
            userDomain = context.Domains.Find(suzerainId.Value);

            return userDomain.PersonId == user.PersonId;
        }

        public static async Task<User> AccessСheckAndGetCurrentUser(ApplicationDbContext _context, UserManager<User> _userManager,
            ClaimsPrincipal userClaimsPrincipal, int? domainId)
        {
            if (domainId == null)
                return null;

            var currentUser = await _userManager.GetCurrentUser(userClaimsPrincipal, _context);
            if (currentUser == null || currentUser.PersonId == null)
                return null;

            return !ValidDomain(_context, currentUser, domainId.Value, out _, out _) 
                    ? null 
                    : currentUser;
        }
    }
}
