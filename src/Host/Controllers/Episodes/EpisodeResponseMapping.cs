using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Aggregates;
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

        public static SlideResponse ToResponse(this Slide source, ColonyStates colonyStats, bool isChange)
        {
            var requirements = source.Buttons.SelectMany(x => x.Requirements).ToList();
            var requirementsResponse = requirements.ToColonyParametersResponse(colonyStats);
            var colonyParameters = source.Parameters.ToResponse(requirements, isChange);

            return new SlideResponse(
                source.Id,
                source.Title,
                source.ImageName,
                source.Text,
                colonyParameters,
                requirementsResponse,
                [.. source.Buttons.Select(x => x.ToResponse(colonyStats))],
                source.TextInput?.ToResponse());
        }

        public static IReadOnlyList<ColonyParameterResponse> ToResponse(
            this IReadOnlyList<KeyValueParameter> source,
            IReadOnlyList<RequirementsParameter>? requirements = null,
            bool isChange = true)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                if (requirements?.Any(x => x.Name == item.Name) ?? false)
                    continue;
                var colonyParameter = item.Name switch
                {
                    StateKeys.ReformPoints.Reserve => ColonyParameterResponse.ActionPoints_Resourses((int)item.Value, isChange),
                    StateKeys.ReformPoints.Income => ColonyParameterResponse.ActionPoints_Trend((int)item.Value, isChange),
                    StateKeys.Solars.Reserve => ColonyParameterResponse.FinanceReserves(item.Value, isChange),
                    StateKeys.Solars.Income => ColonyParameterResponse.FinanceTrend(item.Value, isChange),
                    StateKeys.Mood.Reserve => ColonyParameterResponse.TrustResourse(item.Value, isChange),
                    StateKeys.Modules.Used => ColonyParameterResponse.AreaOccupied((int)item.Value),
                    StateKeys.Population => ColonyParameterResponse.Population((int)item.Value, isChange),
                    _ => null,
                };
                if (colonyParameter == null)
                    continue;
                result.Add(colonyParameter);
            }

            return result;
        }

        public static IReadOnlyList<ColonyParameterResponse> ToColonyParametersResponse(
            this IReadOnlyList<RequirementsParameter> requirements,
            ColonyStates colonyStats)
        {
            var result = new List<ColonyParameterResponse>(requirements.Count);

            foreach (var item in requirements)
            {
                var colonyParameter = item.Name switch
                {
                    StateKeys.ReformPoints.Reserve => RequirementParametersResponse.ActionPoints_Resourses(item.Threshold, item.IsTopThreshold),
                    StateKeys.Solars.Reserve => RequirementParametersResponse.FinanceReserves(item.Threshold, item.IsTopThreshold),
                    StateKeys.Solars.Income => RequirementParametersResponse.FinanceTrend(item.Threshold, item.IsTopThreshold),
                    StateKeys.Mood.Reserve => RequirementParametersResponse.TrustResourse(item.Threshold, item.IsTopThreshold),
                    StateKeys.Modules.Used => RequirementParametersResponse.AreaOccupied(item.Threshold, item.IsTopThreshold),
                    _ => null,
                };
                if (colonyParameter == null)
                    continue;
                var isMet = item.Check(colonyStats);
                colonyParameter.Status = isMet
                    ? ParameterStatusConstants.Good
                    : ParameterStatusConstants.Critical;
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
