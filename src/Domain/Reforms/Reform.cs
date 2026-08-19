using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Domain.Reforms
{
    public class Reform
    {
        public string Code { get; }
        public DisplayInfo DisplayInfo { get; }
        public IReadOnlyList<GameEffect> Changes { get; }
        public IReadOnlyList<GameRequirement> Requirements { get; }

        public Reform(
            string code,
            DisplayInfo displayInfo,
            IReadOnlyList<GameEffect> changes,
            IReadOnlyList<GameRequirement> requirements)
        {
            Code = code;
            DisplayInfo = displayInfo;
            Changes = changes;
            Requirements = requirements;
        }

        internal void SetReform(Colony colony, string? stringValue = null)
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
