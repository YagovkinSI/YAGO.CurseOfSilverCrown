using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.GameActions
{
    public class GameAction
    {
        public IReadOnlyList<GameRequirement> Requirements { get; }
        public IReadOnlyList<GameEffect> Effects { get; }
        public IReadOnlyList<string> NewEventCodes { get; }
        public DisplayInfo? DisplayInfoResult { get; }

        public GameAction(
            IReadOnlyList<GameEffect> effects,
            IReadOnlyList<string> newEventCodes,
            IReadOnlyList<GameRequirement>? requirements = null,
            DisplayInfo? displayInfoResult = null)
        {
            Effects = effects;
            NewEventCodes = newEventCodes;
            Requirements = requirements ?? [];
            DisplayInfoResult = displayInfoResult;
        }

        public void Aplly(Colony colony, string? stringValue = null)
        {
            if (Requirements.Any(x => !x.Check(colony.State)))
                throw new YagoException("Не выполнены условия.");

            foreach (var parameter in Effects)
            {
                parameter.Apply(colony, stringValue);
            }
        }
    }
}
