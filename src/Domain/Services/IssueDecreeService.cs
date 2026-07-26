using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Services
{
    public static class IssueDecreeService
    {
        public static void IssueDecree(this ColonyState colonyState, Decree decree)
        {
            var actionPoints = decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ReformPointsCurrent)?.Value ?? 0;
            if (colonyState.Resources[ColonyResourceType.ReformPoints].Value < -actionPoints)
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == StateKey.SolarsCurrent)?.Value ?? 0;
            if (colonyState.Resources[ColonyResourceType.Solars].Value < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = colonyState.Slots[ColonySlotType.Modules].GetFree(colonyState);
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ModulesUsed)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            foreach (var parameter in decree.Parameters)
            {
                colonyState.AddParameter(parameter.Name, parameter.Value);
            }
        }
    }
}
