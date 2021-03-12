using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Helpers
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
    }
}
