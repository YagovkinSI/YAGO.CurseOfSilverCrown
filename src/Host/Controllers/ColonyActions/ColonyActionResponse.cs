using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.ColonyActions
{
    public class ColonyActionResponse
    {
        public NotificationResponse? Notification { get; }

        public UpdatedColonyEntities UpdatedEntities { get; }

        public ColonyActionResponse(
            NotificationResponse? notification, 
            UpdatedColonyEntities updatedEntities)
        {
            Notification = notification;
            UpdatedEntities = updatedEntities;
        }
    }
}
