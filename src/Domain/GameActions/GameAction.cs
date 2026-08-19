using System.Collections.Generic;

namespace YAGO.World.Domain.GameActions
{
    public class GameAction
    {
        public IReadOnlyList<GameRequirement> Requirements { get; }
        public IReadOnlyList<GameEffect> Changes { get; }
        public IReadOnlyList<string> NewEventCodes { get; }

        public GameAction(
            IReadOnlyList<GameEffect> changes,
            IReadOnlyList<string> newEventCodes,
            IReadOnlyList<GameRequirement>? requirements = null)
        {
            Changes = changes;
            NewEventCodes = newEventCodes;
            Requirements = requirements ?? [];
        }
    }
}
