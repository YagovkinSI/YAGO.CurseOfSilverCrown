using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Decrees
{
    public static class DecreeResponseMapping
    {
        public static DecreeDetails ToMyDataResponse(
            this Decree source,
            ColonyStats colonyStats)
        {
            var colonyParameters = GetColonyParameters(source.Parameters);

            return new DecreeDetails(
                source.Id,
                source.Name,
                source.Image,
                source.Text,
                colonyParameters,
                source.Description,
                source.Button.ToResponse(colonyStats));
        }

        private static IReadOnlyList<ColonyParameterResponse> GetColonyParameters(
            IReadOnlyList<KeyValueParameter> source)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                var colonyParameter = item.Name switch
                {
                    ColonyParameterNames.Economic_Reserves => ColonyParameterResponse.FinanceReserves(item.Value, isChange: true),
                    ColonyParameterNames.Mood_Total => ColonyParameterResponse.TrustResourse(item.Value, isChange: true),
                    _ => null,
                };
                if (colonyParameter != null)
                    result.Add(colonyParameter);
            }

            return result;
        }
    }
}
