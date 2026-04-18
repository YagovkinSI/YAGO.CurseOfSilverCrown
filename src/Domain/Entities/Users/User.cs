using System;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Users
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
        public string UserName { get; private set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; private set; }

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
                userName: $"User_{new Random().Next(0, 99999999)}",
                email: null,
                registeredAtUtc: DateTime.UtcNow,
                lastActivityAtUtc: DateTime.UtcNow,
                isTemporary: true
            );
        }

        public bool TryUpdateLastActivityIfNeeded()
        {
            const int timeoutBetweenUpdateLastActivityInSeconds = 30;
            var coolDown = TimeSpan.FromSeconds(timeoutBetweenUpdateLastActivityInSeconds);
            if (LastActivityAtUtc > DateTime.UtcNow - coolDown)
                return false;

            LastActivityAtUtc = DateTime.UtcNow;
            return true;
        }

        public void ConvertToPermanentAccount(string userName, string? email)
        {
            if (!IsTemporary)
                throw new YagoException("Пользователь уже имеет постоянный аккаунт.");

            UserName = userName;
            Email = email;
            IsTemporary = false;
        }
    }
}
