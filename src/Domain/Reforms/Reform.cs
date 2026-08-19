using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Domain.Reforms
{
    public class Reform
    {
        public string Code { get; }
        public DisplayInfo DisplayInfo { get; }
        public IReadOnlyList<GameParameterChanging> Changes { get; }
        public IReadOnlyList<GameParameterRequirement> Requirements { get; }
        public Action<ColonyState>? AdditionalCheck { get; }

        public Reform(
            string code,
            DisplayInfo displayInfo,
            IReadOnlyList<GameParameterChanging> changes,
            IReadOnlyList<GameParameterRequirement> requirements,
            Action<ColonyState>? additionalCheck)
        {
            Code = code;
            DisplayInfo = displayInfo;
            Changes = changes;
            Requirements = requirements;
            AdditionalCheck = additionalCheck;
        }

        internal void SetReform(Colony colony, string? stringValue = null)
        {
            Check(colony.State);
            foreach (var parameter in Changes)
            {
                parameter.Apply(colony, stringValue);
            }
        }

        private void Check(ColonyState colonyState)
        {
            var actionPoints = Changes.FirstOrDefault(x => x.ParameterType == GameParameterType.ActionPointsCurrent)?.Delta ?? 0;
            if (colonyState.Resources.ActionPoints.Value < -actionPoints)
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = Changes.FirstOrDefault(x => x.ParameterType == GameParameterType.SolarsCurrent)?.Delta ?? 0;
            if (colonyState.Resources.Solars.Value < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = colonyState.Slots[ColonySlotType.Modules].GetFree(colonyState);
            if (zonesAvailable < -(Changes.FirstOrDefault(x => x.ParameterType == GameParameterType.ModulesUsed)?.Delta ?? 0))
                throw new YagoException("Недостаточно секторов.");

            AdditionalCheck?.Invoke(colonyState);
        }
    }
}
