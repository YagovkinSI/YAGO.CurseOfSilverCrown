using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Parameters;

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
                context.Update(user);
                await context.SaveChangesAsync();
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

            domain = GetDomain(context, domainId);

            if (domain.PersonId == user.PersonId)
            {
                userDomain = domain;
                return true;
            }

            var suzerainId = domain.SuzerainId;
            userDomain = GetDomain(context, suzerainId.Value);

            return userDomain.PersonId == user.PersonId;
        }

        private static Domain GetDomain(ApplicationDbContext context, int domainId)
        {
            return context.Domains
                .Where(d => d.Id <= Constants.MaxPlayerCount)
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .Single(d => d.Id == domainId);
        }
    }
}
