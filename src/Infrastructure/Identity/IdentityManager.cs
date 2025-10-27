using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Users;
using YAGO.World.Infrastructure.Database.Colonies;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Identity
{
    internal class IdentityManager : IIdentityManager
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IUserRepository _userRepository;

        public IdentityManager(
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
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

        public async Task<User> ConvertToPermanentAccount(
            long userId,
            string userName,
            string password,
            string? email,
            CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.Find(userId, cancellationToken)
                ?? throw new YagoNotAuthorizedException();
            await ThrowIfUserNotValidForConvertToPermanent(currentUser, userName, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            var currentUserEntity = currentUser.ToEntity();
            var result = await _userManager.AddPasswordAsync(currentUserEntity, password);
            if (!result.Succeeded)
                throw GetException(result.Errors.First().Code);

            await ConvertToPermanentProperties(currentUserEntity, userName, email);

            await _signInManager.RefreshSignInAsync(currentUserEntity);

            return currentUserEntity.ToDomain();
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

        private async Task ConvertToPermanentProperties(UserEntity currentUserEnity, string userName, string? email)
        {
            currentUserEnity.ConvertToPermanentAccount(userName, email);
            var updateResult = await _userManager.UpdateAsync(currentUserEnity);
            if (!updateResult.Succeeded)
                throw new YagoException($"Не удалось преобразовать аккаунт: {string.Join(", ", updateResult.Errors)}");
        }

        private async Task ThrowIfUserNotValidForConvertToPermanent(
            User currentUser,
            string userName,
            CancellationToken cancellationToken)
        {
            if (!currentUser.IsTemporary)
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
