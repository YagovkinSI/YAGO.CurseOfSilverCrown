using System;
using YAGO.World.Domain.Common.Entities.Enums;
using YAGO.World.Domain.Common.Entities.Interfaces;

namespace YAGO.World.Domain.Users
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : IEntity
    {
        public EntityType Type => EntityType.User;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Никнейм
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Дата и время регистрации
        /// </summary>
        public DateTime Registered { get; }

        /// <summary>
        /// Дата и время последней активности
        /// </summary>
        public DateTime LastActivity { get; }

        public User(
            long id,
            string name,
            string email,
            DateTime registered,
            DateTime lastActivity)
        {
            Id = id;
            Name = name;
            Email = email;
            Registered = registered;
            LastActivity = lastActivity;
        }
    }
}
