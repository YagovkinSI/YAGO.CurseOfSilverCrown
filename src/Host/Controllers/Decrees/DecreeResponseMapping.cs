using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
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
            var button = GetButtonResponse(source, colonyStats);

            return new DecreeDetails(
                source.Id,
                source.Name,
                source.Image,
                source.Text,
                colonyParameters,
                source.Description,
                button);
        }

        private static SlideButtonResponse GetButtonResponse(Decree source, ColonyStats colonyStats)
        {
            var (isAvailable, refusalReason) = source.AvailableRequirements.Check(colonyStats);
            var button = new SlideButtonResponse(
                refusalReason ?? "Издать указ",
                isAvailable,
                Action: new SlideButtonActionResponse(
                    SlideButtonActionTypeResponseConstants.Default,
                    EpisodeActionNames.IssueDecree, 
                    [source.Id.ToString()]),
                Navigate: null,
                ToSlide: null);
            return button;
        }

        private static IReadOnlyList<ColonyParameterResponse> GetColonyParameters(
            IReadOnlyList<KeyValueParameter> source)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                var colonyParameter = item.Name switch
                {
                    ColonyStatNames.ActionPoints_Resourses => ColonyParameterResponse.ActionPoints_Resourses((int)item.Value, isChange: true),
                    ColonyStatNames.Economic_Reserves => ColonyParameterResponse.FinanceReserves(item.Value, isChange: true),
                    ColonyStatNames.Mood_Total => ColonyParameterResponse.TrustResourse(item.Value, isChange: true),
                    _ => null,
                };
                if (colonyParameter != null)
                    result.Add(colonyParameter);
            }

            return result;
        }
    }
}
