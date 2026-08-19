using System.Linq;
using YAGO.World.Application.Reforms;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Host.Controllers.Common.GameRequirements;
using YAGO.World.Host.Controllers.Common.GameVisibleEffects;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Reforms
{
    public static class ReformResponseMapping
    {
        public static ReformDetails ToReformDetails(
            this ColonyState colonyState,
            ReformDto reformDto)
        {
            var reform = reformDto.Reform;
            var requirements = reform.Action.Requirements.Select(x => x.ToResponse(colonyState)).ToList();
            var colonyParameters = reform.Action.Changes.ToVisibleEffectsResponse();
            var button = GetButtonResponse(reformDto);

            return new ReformDetails(
                reform.Code,
                reform.DisplayInfo.Name,
                reform.DisplayInfo.ImageName,
                colonyParameters,
                requirements,
                reform.DisplayInfo.Description,
                button);
        }

        private static SlideButtonResponse GetButtonResponse(ReformDto reformDto)
        {
            var isAvailable = reformDto.IsAvailable;
            var button = new SlideButtonResponse(
                "Издать указ",
                isAvailable,
                Action: new SlideButtonActionResponse(
                    SlideButtonActionTypeResponseConstants.Default,
                    EpisodeActionNames.IssueReform,
                    [reformDto.Reform.Code]),
                Navigate: null,
                ToSlide: null,
                InfoSlideId: null);
            return button;
        }
    }
}
