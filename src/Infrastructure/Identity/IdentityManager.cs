using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Domain.Entities.Users;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Colonies;
using YAGO.World.Infrastructure.Database.Users;

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

        public async Task Register(User newUser, string password, CancellationToken cancellationToken)
        {
            var userEntity = newUser.ToEntity();
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.CreateAsync(userEntity, password);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);
        }

        public async Task CreateTemporaryUser(User newUser, CancellationToken cancellationToken)
        {
            var userEntity = newUser.ToEntity();
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.CreateAsync(userEntity);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);
        }

        public async Task ConvertToPermanentAccount(
            User permanentUser,
            string password,
            CancellationToken cancellationToken)
        {
            var source = permanentUser.ToEntity();
            var target = await _userManager.FindByIdAsync(permanentUser.Id.ToString())
                    ?? throw new YagoNotFoundException(nameof(UserEntity), permanentUser.Id.ToString());

            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.AddPasswordAsync(target, password);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);

            EntityUpdater.Update(source, target);
            var updateResult = await _userManager.UpdateAsync(target);
            if (!updateResult.Succeeded)
                throw new YagoException($"Не удалось преобразовать аккаунт: {string.Join(", ", updateResult.Errors)}");

            await _signInManager.RefreshSignInAsync(target);
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
