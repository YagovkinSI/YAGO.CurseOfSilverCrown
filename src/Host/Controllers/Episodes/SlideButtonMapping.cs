using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Host.Controllers.GameActions;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class SlideButtonMapping
    {
        public static SlideButtonResponse ToResponse(this SlideButton source, ColonyState colonyStats, long colonyEventId)
        {
            var isAvailable = !source.Requirements.Any(x => !x.Check(colonyStats));
            return new SlideButtonResponse(
                source.Name,
                isAvailable,
                source.Action?.ToResponse(colonyEventId),
                source.Navigate?.ToResponse(),
                source.ToSlide?.ToResponse(),
                source.InfoSlideId,
                source.Kind.ToResponse());
        }

        private static SlideButtonActionResponse ToResponse(this SlideButtonAction source, long colonyEventId)
        {
            return new SlideButtonActionResponse(
                source.Type == SlideButtonActionType.InputCompleted,
                GameActionType.Event,
                colonyEventId.ToString(),
                source.DilemmaResolving);
        }

        private static SlideButtonNavigateResponse ToResponse(this SlideButtonNavigate source)
        {
            return new SlideButtonNavigateResponse(
                source.ActionUrl);
        }

        private static SlideButtonToSlideResponse ToResponse(this SlideButtonToSlide source)
        {
            return new SlideButtonToSlideResponse(
                source.SlideId);
        }

        private static string ToResponse(this SlideButtonKind kind)
        {
            return kind switch
            {
                SlideButtonKind.Default => SlideButtonKindConstants.Default,
                SlideButtonKind.Reference => SlideButtonKindConstants.Reference,
            };
        }
    }
}
