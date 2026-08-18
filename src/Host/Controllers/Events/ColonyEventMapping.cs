using System.Linq;
using YAGO.World.Application.Events;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events.Models;

namespace YAGO.World.Host.Controllers.Events
{
    public static class ColonyEventMapping
    {
        public static ColonyEventPrivate ToResponse(this ColonyEventPrivateDto source)
        {
            return new ColonyEventPrivate(
                source.ColonyEvent.Id,
                source.GameEvent.Slides[0].Title,
                source.GameEvent.Type.ToResponse(),
                source.ToEpisodeResponse(),
                source.ColonyEvent.IsRead,
                source.ColonyEvent.CreatedAtUtc);
        }

        public static ColonyEventSummary ToResponse(this ColonyEventSummaryDto source)
        {
            return new ColonyEventSummary(
                source.ColonyEvent.Id,
                source.GameEvent.Slides[0].Title,
                source.GameEvent.Type.ToResponse(),
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
