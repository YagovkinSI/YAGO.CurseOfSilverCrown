using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Reforms;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Reforms
{
    public static class ReformResponseMapping
    {
        public static ReformDetails ToReformDetails(
            this ColonyState colonyState,
            ReformDto reformDto)
        {
            var reform = colonyState.GetReform(reformDto.Id);
            var requirements = GetRequirementParameters(reform.Requirements, colonyState);
            var colonyParameters = GetColonyParameters(reform.Parameters, reform.Requirements);
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
            IReadOnlyList<RequirementsParameter> requirements,
            ColonyState colonyStats)
        {
            var result = new List<ColonyParameterResponse>(requirements.Count);

            foreach (var item in requirements)
            {
                var colonyParameter = item.Name switch
                {
                    StateKey.ActionPointsCurrent => RequirementParametersResponse.ActionPoints_Resourses(item.Threshold, item.IsTopThreshold),
                    StateKey.SolarsCurrent => RequirementParametersResponse.FinanceReserves(item.Threshold, item.IsTopThreshold),
                    StateKey.MoodCurrent => RequirementParametersResponse.TrustResourse(item.Threshold, item.IsTopThreshold),
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
            IReadOnlyList<KeyValueParameter> source,
            IReadOnlyList<RequirementsParameter> requirements)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                if (requirements.Any(x => x.Name == item.Name))
                    continue;
                var colonyParameter = item.Name switch
                {
                    StateKey.ActionPointsCurrent => ColonyParameterResponse.ActionPoints_Resourses((int)item.Value, isChange: true),
                    StateKey.SolarsCurrent => ColonyParameterResponse.FinanceReserves(item.Value, isChange: true),
                    StateKey.MoodCurrent => ColonyParameterResponse.TrustResourse(item.Value, isChange: true),
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
