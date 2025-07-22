using YAGO.World.Domain.Story;

namespace YAGO.World.Domain.CurrentUsers
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
        public CurrentUser? User { get; set; }

        /// <summary>
        /// Данные истории текущего пользователя (NULL если пользователь не авторизован)
        /// </summary>
        public StoryNode? StoryNode { get; set; }

        /// <summary>
        /// Создание данных неавторизованного пользователя
        /// </summary>
        public static AuthorizationData NotAuthorized => new(user: null, storyNode: null);

        public AuthorizationData(CurrentUser? user, StoryNode? storyNode)
        {
            User = user;
            StoryNode = storyNode;
        }
    }
}
