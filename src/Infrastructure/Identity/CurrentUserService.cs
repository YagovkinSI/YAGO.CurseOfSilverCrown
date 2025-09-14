using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces;
using YAGO.World.Domain.CurrentUsers;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Identity
{
    internal class IdentityManager : IIdentityManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityManager(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<CurrentUser?> GetCurrentUser(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
                return null;

            var currentUser = user.ToDomainCurrentUser();
            return await Task.FromResult(currentUser!);
        }

        public async Task Register(CurrentUser user, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userDatabase = user.ToDatabase();
            var result = await _userManager.CreateAsync(userDatabase, password);
            if (!result.Succeeded)
                throw GetExtension(result.Errors.First().Code);
        }

        public async Task ChangeRegistration(ClaimsPrincipal claimsPrincipal, string userName, string email, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.GetUserAsync(claimsPrincipal);

            cancellationToken.ThrowIfCancellationRequested();
            user.Email = email;
            var result = await _userManager.SetUserNameAsync(user, userName);
            if (!result.Succeeded)
                throw GetExtension(result.Errors.First().Code);

            result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
                throw GetExtension(result.Errors.First().Code);

            result = await _userManager.AddPasswordAsync(user, password);
            if (!result.Succeeded)
                throw GetExtension(result.Errors.First().Code);
        }

        public async Task Login(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            if (!result.Succeeded)
                throw new YagoException("Ошибка авторизации. Проверьте логин и пароль.");
        }

        public async Task Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _signInManager.SignOutAsync();
        }

        private static YagoException GetExtension(string identityError)
        {
            return identityError switch
            {
                "DuplicateUserName" => new YagoException("Ошибка регистрации. Такой логин уже занят."),
                _ => new YagoException("Ошибка регистрации. Неизвестная ошибка."),
            };
        }
    }
}
