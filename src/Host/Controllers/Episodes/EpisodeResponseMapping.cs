using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Events;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class EpisodeResponseMapping
    {
        public static EpisodeResponse ToEpisodeResponse(this ColonyEventPrivateDto source)
        {
            var eventCode = source.GameEvent.Code;
            return new EpisodeResponse(
                [.. source.GameEvent.Slides.Select(x => x.ToResponse(source.ColonyState, isChange: true, eventCode))]);
        }

        public static SlideResponse ToResponse(this Slide source, ColonyState colonyStats, bool isChange, string eventCode)
        {
            var requirements = source.Buttons.SelectMany(x => x.Requirements).ToList();
            var requirementsResponse = requirements.ToColonyParametersResponse(colonyStats);
            var colonyParameters = source.ParameterChanges.ToResponse(requirements, isChange);

            return new SlideResponse(
                source.Id,
                source.Title,
                source.ImageName,
                source.Text,
                colonyParameters,
                requirementsResponse,
                [.. source.Buttons.Select(x => x.ToResponse(colonyStats, eventCode))],
                source.TextInput?.ToResponse());
        }

        public static IReadOnlyList<ColonyParameterResponse> ToResponse(
            this IReadOnlyList<GameParameterChanging> source,
            IReadOnlyList<GameParameterRequirement>? requirements = null,
            bool isChange = true)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                if (requirements?.Any(x => x.ParameterType == item.ParameterType) ?? false)
                    continue;
                var colonyParameter = item.ParameterType switch
                {
                    GameParameterType.ActionPointsCurrent => ColonyParameterResponse.ActionPoints_Resourses((int)item.Delta!.Value, isChange),
                    GameParameterType.ActionPointsDelta => ColonyParameterResponse.ActionPoints_Trend((int)item.Delta!.Value, isChange),
                    GameParameterType.SolarsCurrent => ColonyParameterResponse.FinanceReserves(item.Delta!.Value, isChange),
                    GameParameterType.SolarsDelta => ColonyParameterResponse.FinanceTrend(item.Delta!.Value, isChange),
                    GameParameterType.MoodCurrent => ColonyParameterResponse.TrustResourse(item.Delta!.Value, isChange),
                    GameParameterType.ModulesUsed => ColonyParameterResponse.AreaOccupied((int)item.Delta!.Value),
                    GameParameterType.Population => ColonyParameterResponse.Population((int)item.Delta!.Value, isChange),
                    _ => null,
                };
                if (colonyParameter == null)
                    continue;
                result.Add(colonyParameter);
            }

            return result;
        }

        public static IReadOnlyList<ColonyParameterResponse> ToColonyParametersResponse(
            this IReadOnlyList<GameParameterRequirement> requirements,
            ColonyState colonyStats)
        {
            var result = new List<ColonyParameterResponse>(requirements.Count);

            foreach (var item in requirements)
            {
                var colonyParameter = item.ParameterType switch
                {
                    GameParameterType.ActionPointsCurrent => RequirementParametersResponse.ActionPoints_Resourses(item.Threshold, item.IsLessThan),
                    GameParameterType.SolarsCurrent => RequirementParametersResponse.FinanceReserves(item.Threshold, item.IsLessThan),
                    GameParameterType.SolarsDelta => RequirementParametersResponse.FinanceTrend(item.Threshold, item.IsLessThan),
                    GameParameterType.MoodCurrent => RequirementParametersResponse.TrustResourse(item.Threshold, item.IsLessThan),
                    GameParameterType.ModulesUsed => RequirementParametersResponse.AreaOccupied(item.Threshold, item.IsLessThan),
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
