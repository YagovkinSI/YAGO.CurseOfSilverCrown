using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Infrastructure.Database.Models.Users.Mappings;

namespace YAGO.World.Infrastructure.Identity
{
    internal class IdentityManager : IIdentityManager
    {
        private readonly UserManager<Database.Models.Users.UserEntity> _userManager;
        private readonly SignInManager<Database.Models.Users.UserEntity> _signInManager;

        public IdentityManager(
            UserManager<Database.Models.Users.UserEntity> userManager,
            SignInManager<Database.Models.Users.UserEntity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Domain.Users.User?> GetCurrentUser(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            return user == null ? null : user.ToDomain();
        }

        public async Task Register(Domain.Users.User user, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userDatabase = user.ToEntity();
            var result = await _userManager.CreateAsync(userDatabase, password);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);
        }

        public async Task ChangeLogin(ClaimsPrincipal claimsPrincipal, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
                throw new YagoNotAuthorizedException();

            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.SetUserNameAsync(user, userName);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);
        }

        public async Task ChangePassword(ClaimsPrincipal claimsPrincipal, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
                throw new YagoNotAuthorizedException();

            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);

            result = await _userManager.AddPasswordAsync(user, password);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);
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

        private static YagoException GetException(string identityError)
        {
            return identityError switch
            {
                "DuplicateUserName" => new YagoException("Ошибка регистрации. Такой логин уже занят."),
                _ => new YagoException("Ошибка регистрации. Неизвестная ошибка."),
            };
        }
    }
}
