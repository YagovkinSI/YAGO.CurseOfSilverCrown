using System.Collections.Generic;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    public class CycleParameters
    {
        public IReadOnlyList<string> GameEventsIds { get; }

        public CycleParameters(
            IReadOnlyList<string> gameEventsIds)
        {
            GameEventsIds = gameEventsIds;
        }
    }
}
