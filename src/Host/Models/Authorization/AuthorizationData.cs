namespace YAGO.World.Host.Models.Authorization
{
    /// <summary>
	/// Данные авторизации
	/// </summary>
	public class AuthorizationData
    {
        /// <summary>
        /// Флаг, указывающий на наличие авторизации пользователя
        /// </summary>
        public bool IsAuthorized => User != null;

        /// <summary>
        /// Данные авторизованного пользователя (NULL если пользователь не авторизован)
        /// </summary>
        public UserPrivate User { get; set; }

        /// <summary>
        /// Создание данных неавторизованного пользователя
        /// </summary>
        public static AuthorizationData NotAuthorized => new(user: null);

        public AuthorizationData(UserPrivate user)
        {
            User = user;
        }
    }
}
