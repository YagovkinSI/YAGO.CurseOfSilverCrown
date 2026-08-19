using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;

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

        internal void Aplly(Colony colony, string? stringValue = null)
        {
            foreach (var requirement in Requirements)
            {
                if (!requirement.Check(colony.State))
                    throw new YagoException("Не выполнены условия.");
            }

            foreach (var parameter in Changes)
            {
                parameter.Apply(colony, stringValue);
            }
        }
    }
}
