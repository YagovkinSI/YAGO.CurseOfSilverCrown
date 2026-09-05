using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Reforms;
using YAGO.World.Domain.Colonies;
using YAGO.World.Host.Controllers.Common.GameRequirements;
using YAGO.World.Host.Controllers.Common.GameVisibleEffects;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.GameActions;

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
            var colonyParameters = reform.Action.Effects.ToVisibleEffectsResponse();
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

        public static IReadOnlyList<ReformSummary> ToResponse(
            this IEnumerable<ReformDto> reformDtos)
        {
            var result = new List<ReformSummary>();
            foreach (var reformDto in reformDtos)
            {
                var reform = reformDto.Reform;
                var response = new ReformSummary(
                    reform.Code,
                    reform.DisplayInfo.Name,
                    reformDto.IsAvailable);
                result.Add(response);
            }
            return result;
        }

        private static SlideButtonResponse GetButtonResponse(ReformDto reformDto)
        {
            var isAvailable = reformDto.IsAvailable;
            return reformDto.Reform.Action.Effects.Any(x => x.NeedInputText)
                ? CreateInputTextButton(reformDto, isAvailable)
                : CreateDefaultButton(reformDto, isAvailable);
        }

        private static SlideButtonResponse CreateInputTextButton(ReformDto reformDto, bool isAvailable)
        {
            return new SlideButtonResponse(
                "Применить",
                isAvailable,
                Action: new SlideButtonActionResponse(
                    true,
                    GameActionType.Reform,
                    reformDto.Reform.Code,
                    string.Empty),
                Navigate: null,
                ToSlide: null,
                InfoSlideId: null);
        }

        private static SlideButtonResponse CreateDefaultButton(ReformDto reformDto, bool isAvailable)
        {
            return new SlideButtonResponse(
                "Издать указ",
                isAvailable,
                Action: new SlideButtonActionResponse(
                    false,
                    GameActionType.Reform,
                    reformDto.Reform.Code,
                    string.Empty),
                Navigate: null,
                ToSlide: null,
                InfoSlideId: null);
        }
    }
}
