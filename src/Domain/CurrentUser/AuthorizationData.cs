using YAGO.World.Domain.Factions;
using YAGO.World.Domain.Users;

namespace YAGO.World.Domain.CurrentUser
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
        public User? User { get; set; }

        /// <summary>
        /// Данные фракции пользователя (NULL если нет)
        /// </summary>
        public Faction? Faction { get; set; }

        /// <summary>
        /// Создание данных неавторизованного пользователя
        /// </summary>
        public static AuthorizationData NotAuthorized => new(user: null, faction: null);

        public AuthorizationData(User? user, Faction? faction)
        {
            User = user;
            Faction = faction;
        }
    }
}
