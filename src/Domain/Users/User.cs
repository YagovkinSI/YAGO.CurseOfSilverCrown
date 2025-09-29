using System;
using YAGO.World.Domain.Common.Entities.Interfaces;

namespace YAGO.World.Domain.Users
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Уникальное имя пользователя (логин)
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; }

        /// <summary>
        /// Дата и время регистрации
        /// </summary>
        public DateTime RegisteredAtUtc { get; }

        /// <summary>
        /// Дата и время последней активности
        /// </summary>
        public DateTime LastActivityAtUtc { get; }

        /// <summary>
        /// Флаг отображающий временные аккаунты без пароля
        /// </summary>
        public bool IsTemporary { get; }

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
    }
}
