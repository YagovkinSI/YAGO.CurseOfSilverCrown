using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.APIModels;
using YSI.CurseOfSilverCrown.Core.Database;
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

                var userJson = user.UserJsonDeserialized;
                userJson.LastActivity = DateTimeOffset.Now;
                user.UserJsonDeserialized = userJson;

                context.Update(user);
                await context.SaveChangesAsync();
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

        public static async Task<Response<UserPrivate>> RegisterAsync(ApplicationDbContext context,
            UserManager<User> userManager, SignInManager<User> signInManager, string userName, string password)
        {
            var userJson = new UserJson
            {
                Created = DateTimeOffset.Now,
                LastActivity = DateTimeOffset.Now,
            };

            var user = new User
            {
                Email = string.Empty,
                UserName = userName,
                UserJsonDeserialized = userJson,
                LastActivityTime = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return new Response<UserPrivate>(string.Join(". ", result.Errors.Select(e => e.Description)));

            await signInManager.SignInAsync(user, true);

            var userPirvate = await context.Users.GetUserPrivateAsync(user.Id);
            return new Response<UserPrivate>(userPirvate);
        }

        public static async Task<Response<UserPrivate>> LoginAsync(
            ApplicationDbContext context, SignInManager<User> signInManager, string userName, string password)
        {
            var result =
                await signInManager.PasswordSignInAsync(userName, password, true, false);

            if (!result.Succeeded)
                return new Response<UserPrivate>("Неверный логин или пароль");

            var user = await signInManager.UserManager.FindByNameAsync(userName);
            await user.UpdateLastActivityAsync(context);

            var userPirvate = await context.Users.GetUserPrivateAsync(user.Id);
            return new Response<UserPrivate>(userPirvate);
        }

        public static async Task UpdateLastActivityAsync(this User user, ApplicationDbContext context)
        {
            user.LastActivityTime = DateTime.UtcNow;

            var userJson = user.UserJsonDeserialized;
            userJson.LastActivity = DateTimeOffset.Now;
            user.UserJsonDeserialized = userJson;

            context.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
