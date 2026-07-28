using System.Collections.Generic;
using System.Linq;
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
            ColonyState colonyStats)
        {
            var requirements = GetRequirementParameters(source.Requirements, colonyStats);
            var colonyParameters = GetColonyParameters(source.Parameters, source.Requirements);
            var button = GetButtonResponse(source, colonyStats);

            return new DecreeDetails(
                source.Id,
                source.Name,
                source.Image,
                source.Text,
                colonyParameters,
                requirements,
                source.Description,
                button);
        }

        private static SlideButtonResponse GetButtonResponse(Decree source, ColonyState colonyStats)
        {
            var isAvailable = !source.Requirements.Any(x => !x.Check(colonyStats));
            var button = new SlideButtonResponse(
                "Издать указ",
                isAvailable,
                Action: new SlideButtonActionResponse(
                    SlideButtonActionTypeResponseConstants.Default,
                    EpisodeActionNames.IssueDecree,
                    [source.Id.ToString()]),
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
                    StateKey.ReformPointsCurrent => RequirementParametersResponse.ActionPoints_Resourses(item.Threshold, item.IsTopThreshold),
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
                    StateKey.ReformPointsCurrent => ColonyParameterResponse.ActionPoints_Resourses((int)item.Value, isChange: true),
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
