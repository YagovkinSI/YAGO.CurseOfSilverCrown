using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class SlideButtonMapping
    {
        public static SlideButtonResponse ToResponse(this SlideButton source, ColonyStats colonyStats)
        {
            var (isAvailable, refusalReason) = source.AvailableRequirements.Check(colonyStats);

            return new SlideButtonResponse(
                refusalReason ?? source.Name,
                isAvailable,
                source.Action?.ToResponse(),
                source.Navigate?.ToResponse(),
                source.ToSlide?.ToResponse());
        }

        private static SlideButtonActionResponse ToResponse(this SlideButtonAction source)
        {
            return new SlideButtonActionResponse(
                source.ActionName,
                source.Arguments);
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
    }
}
