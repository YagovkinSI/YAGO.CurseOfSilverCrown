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
        public long Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        public User(
            long id,
            string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
