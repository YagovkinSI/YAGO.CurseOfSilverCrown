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

        public User(
            string id,
            string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
