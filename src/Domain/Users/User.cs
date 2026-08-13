using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Users
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : IEntity<long>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Уникальное имя пользователя (логин)
        /// </summary>
        public string UserName
        {
            get => _userName;
            private set
            {
                ValidateUserName(value);
                _userName = value;
            }
        }
        private string _userName = null!;

        /// <summary>
        /// Email
        /// </summary>
        public string? Email
        {
            get => _email;
            private set
            {
                ValidateEmail(value);
                _email = value;
            }
        }
        private string? _email;

        /// <summary>
        /// Дата и время регистрации
        /// </summary>
        public DateTime RegisteredAtUtc { get; }

        /// <summary>
        /// Дата и время последней активности
        /// </summary>
        public DateTime LastActivityAtUtc { get; private set; }

        /// <summary>
        /// Флаг отображающий временные аккаунты без пароля
        /// </summary>
        public bool IsTemporary { get; private set; }

        public User(
            long id,
            string userName,
            string? email,
            DateTime registeredAtUtc,
            DateTime lastActivityAtUtc,
            bool isTemporary)
        {
            Id = id;
            UserName = userName;
            Email = email;
            RegisteredAtUtc = registeredAtUtc;
            LastActivityAtUtc = lastActivityAtUtc;
            IsTemporary = isTemporary;
        }

        public static User CreateNew(
            string userName,
            string? email)
        {
            return new User(
                id: default,
                userName: userName,
                email: email,
                registeredAtUtc: DateTime.UtcNow,
                lastActivityAtUtc: DateTime.UtcNow,
                isTemporary: false
            );
        }

        public static User CreateTemporary()
        {
            return new User(
                id: default,
                userName: $"User_{Random.Shared.Next(0, 99999999)}",
                email: null,
                registeredAtUtc: DateTime.UtcNow,
                lastActivityAtUtc: DateTime.UtcNow,
                isTemporary: true
            );
        }

        public void UpdateLastActivity()
        {
            LastActivityAtUtc = DateTime.UtcNow;
        }

        public void ConvertToPermanentAccount(string userName, string? email)
        {
            if (!IsTemporary)
                throw new YagoNotValidException("Пользователь уже имеет постоянный аккаунт.");

            UserName = userName;
            Email = email;
            IsTemporary = false;
        }

        private static void ValidateUserName(string userName)
        {
            const int MinUserNameLength = 3;
            const int MaxUserNameLength = 20;

            if (string.IsNullOrWhiteSpace(userName))
                throw new YagoNotValidException("Имя пользователя не может быть пустым.");

            if (userName.Length < MinUserNameLength)
                throw new YagoNotValidException("Логин должен содержать не менее 3 символов.");
            else if (userName.Length > MaxUserNameLength)
                throw new YagoNotValidException("Логин должен содержать не более 20 символов.");

            var errorList = new List<string>();
            if (!Regex.IsMatch(userName, "^[a-zA-Z0-9_-]+$"))
                errorList.Add("Логин может содержать только латинские буквы, цифры, подчеркивание (_) и дефис (-).");

            if (!Regex.IsMatch(userName, "[a-zA-Z]"))
                errorList.Add("Логин должен содержать хотя бы одну латинскую букву.");

            if (errorList.Count != 0)
                throw new YagoNotValidException(string.Join(" ", errorList));
        }

        private static void ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return;

            if (!Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
                throw new YagoNotValidException("Некорректный формат электронной почты.");
        }
    }
}
