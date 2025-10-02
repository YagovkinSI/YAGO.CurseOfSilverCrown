using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users.Interfaces;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Users;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Database.Models.Users.Mappings;

namespace YAGO.World.Infrastructure.Identity
{
    internal class IdentityManager : IIdentityManager
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;

        public IdentityManager(
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User?> GetCurrentUser(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUserEntity(claimsPrincipal, cancellationToken);
            return user?.ToDomain();
        }

        public async Task Register(string userName, string password, string? email, CancellationToken cancellationToken)
        {
            var userDatabase = UserEntity.CreateNew(userName, email);
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.CreateAsync(userDatabase, password);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);
        }

        public async Task<User> CreateTemporaryUser(CancellationToken cancellationToken)
        {
            var userDatabase = UserEntity.CreateTemporary();
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.CreateAsync(userDatabase);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);

            return userDatabase.ToDomain();
        }

        public async Task<User> ConvertToPermanentAccount(
            ClaimsPrincipal claimsPrincipal,
            string userName,
            string password,
            string? email,
            CancellationToken cancellationToken)
        {
            var currentUserEnity = await GetCurrentUserEntity(claimsPrincipal, cancellationToken)
                ?? throw new YagoNotAuthorizedException();
            await ThrowIfUserNotValidForConvertToPermanent(currentUserEnity, userName, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.AddPasswordAsync(currentUserEnity, password);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);

            await ConvertToPermanentProperties(currentUserEnity, userName, email);

            await _signInManager.RefreshSignInAsync(currentUserEnity);

            return currentUserEnity.ToDomain();
        }

        public async Task Login(string userName, string? password, CancellationToken cancellationToken)
        {
            if (password == null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var user = await _userManager.FindByNameAsync(userName)
                    ?? throw new YagoException(string.Format("Пользователь с именем {0} отсутствует", userName));
                if (!user.IsTemporary)
                    throw new YagoException("Пользователь имеет постоянный аккаунт. Требуется пароль.");

                cancellationToken.ThrowIfCancellationRequested();
                await _signInManager.SignInAsync(user, isPersistent: true);
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
                if (!result.Succeeded)
                    throw new YagoException("Ошибка авторизации. Проверьте логин и пароль.");
            }
        }

        public async Task Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _signInManager.SignOutAsync();
        }

        private async Task<UserEntity?> GetCurrentUserEntity(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userManager.GetUserAsync(claimsPrincipal);
        }

        private async Task ConvertToPermanentProperties(UserEntity currentUserEnity, string userName, string? email)
        {
            currentUserEnity.ConvertToPermanentAccount(userName, email);
            var updateResult = await _userManager.UpdateAsync(currentUserEnity);
            if (!updateResult.Succeeded)
                throw new YagoException($"Не удалось преобразовать аккаунт: {string.Join(", ", updateResult.Errors)}");
        }

        private async Task ThrowIfUserNotValidForConvertToPermanent(
            UserEntity currentUserEnity,
            string userName,
            CancellationToken cancellationToken)
        {
            if (!currentUserEnity.IsTemporary)
                throw new YagoException("Пользователь уже имеет постоянный аккаунт.");

            cancellationToken.ThrowIfCancellationRequested();
            var isUserNameTaken = await _userManager.FindByNameAsync(userName) != null;
            if (isUserNameTaken)
                throw new YagoException("Имя пользователя уже занято");
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
