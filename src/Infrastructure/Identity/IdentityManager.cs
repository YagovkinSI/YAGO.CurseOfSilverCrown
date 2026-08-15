using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Users;
using YAGO.World.Infrastructure.Database.Colonies;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Identity
{
    internal class IdentityManager : IIdentityManager
    {
        internal const int PasswordRequiredLength = 6;

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
                throw GetException(result.Errors.Select(x => x.Code));
        }

        public async Task CreateTemporaryUser(User newUser, CancellationToken cancellationToken)
        {
            var userEntity = newUser.ToEntity();
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.CreateAsync(userEntity);
            if (!result.Succeeded)
                throw GetException(result.Errors.Select(x => x.Code));
        }

        public async Task ConvertToPermanentAccount(
            User permanentUser,
            string password,
            CancellationToken cancellationToken)
        {
            var target = await _userManager.FindByIdAsync(permanentUser.Id.ToString())
                    ?? throw new YagoNotFoundException(nameof(UserEntity), permanentUser.Id.ToString());

            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userManager.AddPasswordAsync(target, password);
            if (!result.Succeeded)
                throw GetException(result.Errors.Select(x => x.Code));

            target.UserName = permanentUser.UserName;
            target.Email = permanentUser.Email;
            target.SetIsTemporary(false);
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

        private static YagoException GetException(IEnumerable<string> identityErrors)
        {
            var errorList = new List<string>();
            foreach (var error in identityErrors)
            {
                var message = GetMessage(error);
                if (message != null)
                    errorList.Add(message);
            }

            if (!errorList.Any())
                errorList.Add("Ошибка регистрации. Неизвестная ошибка.");

            return new YagoNotValidException(string.Join(" ", errorList));
        }

        private static string? GetMessage(string error)
        {
            return error switch
            {
                // Ошибки имени пользователя
                "DuplicateUserName" => "Ошибка регистрации. Такой логин уже занят.",
                "InvalidUserName" => "Ошибка регистрации. Логин содержит недопустимые символы.",

                // Ошибки пароля
                "PasswordTooShort" => $"Пароль должен содержать не менее {PasswordRequiredLength} символов.",
                "PasswordTooLong" => "Пароль должен содержать не более 100 символов.", // Identity по умолчанию 100
                "PasswordRequiresLower" => "Пароль должен содержать строчную латинскую букву.",
                "PasswordRequiresUpper" => "Пароль должен содержать заглавную латинскую букву.",
                "PasswordRequiresDigit" => "Пароль должен содержать цифру.",
                "PasswordRequiresNonAlphanumeric" => "Пароль должен содержать специальный символ (например, !@#$%^&*).",
                "PasswordRequiresUniqueChars" => "Пароль должен содержать уникальные символы.",

                // Ошибки email
                "InvalidEmail" => "Некорректный формат электронной почты.",
                "DuplicateEmail" => "Ошибка регистрации. Такой email уже занят.",

                // Прочие
                "DefaultError" => "Ошибка регистрации. Попробуйте позже.",
                _ => null,
            };
        }
    }
}
