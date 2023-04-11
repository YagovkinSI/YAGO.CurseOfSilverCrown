using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public static async Task<Response<User>> RegisterAsync(
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
                return new Response<User>(string.Join(". ", result.Errors.Select(e => e.Description)));

            await signInManager.SignInAsync(user, true);
            return new Response<User>(user);
        }

        public static async Task<Response<User>> LoginAsync(
            ApplicationDbContext context, SignInManager<User> signInManager, string userName, string password)
        {
            var result =
                await signInManager.PasswordSignInAsync(userName, password, true, false);

            if (!result.Succeeded)
                return new Response<User>("Неверный логин или пароль");

            var user = await signInManager.UserManager.FindByNameAsync(userName);
            await user.UpdateLastActivityAsync(context);
            return new Response<User>(user);
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

        public static async Task<Response<User>> SignOutAsync(ApplicationDbContext context, 
            UserManager<User> userManager, SignInManager<User> signInManager, ClaimsPrincipal claimsPrincipal)
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
                return new Response<User>("Пользователь не найден");

            await user.UpdateLastActivityAsync(context);
            await signInManager.SignOutAsync();
            return new Response<User>(user); ;
        }
    }
}
