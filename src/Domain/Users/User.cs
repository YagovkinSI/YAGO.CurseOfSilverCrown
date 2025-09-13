using System;

namespace YAGO.World.Domain.Users
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        public DateTime LastActivity { get; }

        public User(
            string id,
            string userName,
            DateTime lastActivity)
        {
            Id = id;
            UserName = userName;
            LastActivity = lastActivity;
        }
    }
}
