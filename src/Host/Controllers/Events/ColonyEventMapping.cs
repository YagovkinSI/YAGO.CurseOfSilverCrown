using System.Linq;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events.Models;

namespace YAGO.World.Host.Controllers.Events
{
    public static class ColonyEventMapping
    {
        public static ColonyEventResponse ToMyQuest(this ColonyEventDto source)
        {
            var gameEvent = source.GameEvent;

            return new ColonyEventResponse(
                source.ColonyEvent.Id,
                gameEvent.Slides[0].Title,
                gameEvent.Type.ToResponse(),
                source.ToEpisodeResponse(),
                source.ColonyEvent.IsRead,
                source.ColonyEvent.CreatedAtUtc);
        }

        public static EventResultSlideResponse? ToResponse(this GameActionResult source)
        {
            var colonyPatameters = source.MainParametersResult.Select(x => x.MapToColonyPatameters()).ToList();

            return new EventResultSlideResponse(
                source.Title,
                source.ImageName,
                source.Text,
                colonyPatameters);
        }

        private static string ToResponse(this EventType eventType)
        {
            return eventType switch
            {
                EventType.Default => EventTypeConstants.Default,
                EventType.Autostart => EventTypeConstants.Autostart,
                EventType.Urgent => EventTypeConstants.Urgent,
                EventType.Quest => EventTypeConstants.Quest,
                _ => EventTypeConstants.Default,
            };
        }
    }
}
