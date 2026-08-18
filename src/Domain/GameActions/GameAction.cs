using System.Collections.Generic;

namespace YAGO.World.Domain.GameActions
{
    public class GameAction
    {
        public IReadOnlyList<GameParameterRequirement> Requirements { get; }
        public IReadOnlyList<GameParameterChanging> Changes { get; }
        public IReadOnlyList<string> NewEventCodes { get; }

        public GameAction(
            IReadOnlyList<GameParameterChanging> changes,
            IReadOnlyList<string> newEventCodes,
            IReadOnlyList<GameParameterRequirement>? requirements = null)
        {
            Changes = changes;
            NewEventCodes = newEventCodes;
            Requirements = requirements ?? [];
        }
    }
}
