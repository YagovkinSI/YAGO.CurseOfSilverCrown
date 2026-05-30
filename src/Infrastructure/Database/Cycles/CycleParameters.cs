using System.Collections.Generic;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    public class CycleParameters
    {
        public string? ActiveEventId { get; private set; }
        public IReadOnlyList<string> GameEventsIds { get; }

        public CycleParameters(
            string? activeEventId, 
            IReadOnlyList<string> gameEventsIds)
        {
            ActiveEventId = activeEventId;
            GameEventsIds = gameEventsIds;
        }
    }
}
