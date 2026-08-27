using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Slots;

namespace YAGO.World.Domain.GameParameters
{
    public static class GameParameterValueHelper
    {
        public static double GetValue(this Colony colony, GameParameterType parameterType)
        {
            var colonyState = colony.State;
            return parameterType switch
            {
                GameParameterType.SolarsCurrent => colonyState.Resources.Solars.Value,

                GameParameterType.TurnsCurrent => colonyState.Resources.TurnNumber.Value,

                GameParameterType.MiningSlotsFree => colonyState.Slots[ColonySlotType.Mining].GetFree(colonyState),

                GameParameterType.Population => colonyState.GetPopulation(),
            };
        }
    }
}
