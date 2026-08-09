using System;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Reforms;

namespace YAGO.World.Domain.Services
{
    public static class IssueDecreeService
    {
        public static void IssueDecree(this ColonyState colonyState, Decree decree)
        {
            var actionPoints = decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ActionPointsCurrent)?.Value ?? 0;
            if (colonyState.Resources[ColonyResourceType.ActionPoints].Value < -actionPoints)
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == StateKey.SolarsCurrent)?.Value ?? 0;
            if (colonyState.Resources[ColonyResourceType.Solars].Value < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = colonyState.Slots[ColonySlotType.Modules].GetFree(colonyState);
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ModulesUsed)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            if (decree.Id == 4)
                CheckAndAddPublicDebt(colonyState, solarResservesParameter);

            foreach (var parameter in decree.Parameters)
            {
                colonyState.AddParameter(parameter.Name, parameter.Value);
            }
        }

        private static void CheckAndAddPublicDebt(ColonyState colonyState, double delta)
        {
            var publicDebtContext = new PublicDebtContext(colonyState.GetYagoLevel());
            var publicDebt = new PublicDebt(colonyState.Reforms[ColonyReformType.PublicDebt].Value, publicDebtContext);
            if (!publicDebt.Check(delta))
                throw new YagoException("Получен отказ из-за недостаточного рейинга.");
        }
    }
}
