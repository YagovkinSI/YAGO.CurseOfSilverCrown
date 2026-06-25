using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class EpisodeResponseMapping
    {
        public static EpisodeResponse ToResponse(this ColonyEpisode source)
        {
            return new EpisodeResponse(
                [.. source.Episode.Slides.Select(x => x.ToResponse(source.ColonyStats, isChange: true))]);
        }

        public static SlideResponse ToResponse(this Slide source, ColonyStats colonyStats, bool isChange)
        {
            var colonyParameters = GetColonyParameters(source.Parameters, isChange);

            return new SlideResponse(
                source.Id,
                source.Title,
                source.ImageName,
                source.Text,
                colonyParameters,
                [.. source.Buttons.Select(x => x.ToResponse(colonyStats))],
                source.TextInput?.ToResponse());
        }

        private static IReadOnlyList<ColonyParameterResponse> GetColonyParameters(IReadOnlyList<KeyValueParameter> source, bool isChange = true)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                var colonyParameter = item.Name switch
                {
                    ColonyStatNames.ActionPoints_Resourses => ColonyParameterResponse.ActionPoints_Resourses((int)item.Value, isChange),
                    ColonyStatNames.ActionPoints_Trend => ColonyParameterResponse.ActionPoints_Trend((int)item.Value, isChange),
                    ColonyStatNames.Economic_Reserves => ColonyParameterResponse.FinanceReserves(item.Value, isChange),
                    ColonyStatNames.Economic_Budget_Balance => ColonyParameterResponse.FinanceTrend(item.Value, isChange),
                    ColonyStatNames.Mood_Total => ColonyParameterResponse.TrustResourse(item.Value, isChange),
                    ColonyStatNames.AreaCapacity_Occupied => ColonyParameterResponse.AreaOccupied((int)item.Value),
                    ColonyStatNames.Population_Total => ColonyParameterResponse.Population((int)item.Value, isChange),
                    _ => null,
                };
                if (colonyParameter != null)
                    result.Add(colonyParameter);
            }

            return result;
        }

        private static TextInputResponse ToResponse(this SlideTextInput source)
        {
            return new TextInputResponse();
        }
    }
}
