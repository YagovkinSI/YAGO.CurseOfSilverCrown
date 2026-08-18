using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Reforms;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Reforms;

namespace YAGO.World.Host.Controllers.Decrees
{
    public static class ReformResponseMapping
    {
        public static ReformDetails ToReformDetails(
            this ColonyState colonyState,
            ReformDto reformDto)
        {
            var reform = colonyState.GetReform(reformDto.Id);
            var requirements = GetRequirementParameters(reform.Requirements, colonyState);
            var colonyParameters = GetColonyParameters(reform.Changes, reform.Requirements);
            var button = GetButtonResponse(reformDto, colonyState);

            return new ReformDetails(
                reform.Id,
                reform.Name,
                reform.Image,
                reform.Text,
                colonyParameters,
                requirements,
                reform.Description,
                button);
        }

        private static SlideButtonResponse GetButtonResponse(ReformDto reformDto, ColonyState colonyStats)
        {
            var isAvailable = reformDto.IsAvailable;
            var button = new SlideButtonResponse(
                "Издать указ",
                isAvailable,
                Action: new SlideButtonActionResponse(
                    SlideButtonActionTypeResponseConstants.Default,
                    EpisodeActionNames.IssueReform,
                    [reformDto.Id.ToString()]),
                Navigate: null,
                ToSlide: null,
                InfoSlideId: null);
            return button;
        }

        private static IReadOnlyList<ColonyParameterResponse> GetRequirementParameters(
            IReadOnlyList<GameParameterRequirement> requirements,
            ColonyState colonyStats)
        {
            var result = new List<ColonyParameterResponse>(requirements.Count);

            foreach (var item in requirements)
            {
                var colonyParameter = item.ParameterType switch
                {
                    GameParameterType.ActionPointsCurrent => RequirementParametersResponse.ActionPoints_Resourses(item.Threshold, item.IsLessThan),
                    GameParameterType.SolarsCurrent => RequirementParametersResponse.FinanceReserves(item.Threshold, item.IsLessThan),
                    GameParameterType.MoodCurrent => RequirementParametersResponse.TrustResourse(item.Threshold, item.IsLessThan),
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

        private static IReadOnlyList<ColonyParameterResponse> GetColonyParameters(
            IReadOnlyList<GameParameterChanging> source,
            IReadOnlyList<GameParameterRequirement> requirements)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                if (requirements.Any(x => x.ParameterType == item.ParameterType))
                    continue;
                var colonyParameter = item.ParameterType switch
                {
                    GameParameterType.ActionPointsCurrent => ColonyParameterResponse.ActionPoints_Resourses((int)item.Delta!.Value, isChange: true),
                    GameParameterType.SolarsCurrent => ColonyParameterResponse.FinanceReserves(item.Delta!.Value, isChange: true),
                    GameParameterType.MoodCurrent => ColonyParameterResponse.TrustResourse(item.Delta!.Value, isChange: true),
                    _ => null,
                };
                if (colonyParameter == null)
                    continue;
                result.Add(colonyParameter);
            }

            return result;
        }
    }
}
