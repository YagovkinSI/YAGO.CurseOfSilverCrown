using YAGO.World.Domain.Notifications;

namespace YAGO.World.Host.Controllers.Common
{
    public static class NotificationResponseMapping
    {
        public static NotificationResponse ToResponse(this Notification source)
        {
            return new NotificationResponse(
                source.Title,
                source.Illustration,
                source.Text,
                source.Parameters);
        }
    }
}
