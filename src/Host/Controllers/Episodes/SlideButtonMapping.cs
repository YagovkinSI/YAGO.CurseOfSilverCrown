using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Episodes;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class SlideButtonMapping
    {
        public static SlideButtonResponse ToResponse(this SlideButton source, ColonyState colonyStats)
        {
            var isAvailable = !source.Requirements.Any(x => !x.Check(colonyStats));
            return new SlideButtonResponse(
                source.Name,
                isAvailable,
                source.Action?.ToResponse(),
                source.Navigate?.ToResponse(),
                source.ToSlide?.ToResponse(),
                source.InfoSlideId);
        }

        private static SlideButtonActionResponse ToResponse(this SlideButtonAction source)
        {
            var type = source.Type switch
            {
                SlideButtonActionType.Default => SlideButtonActionTypeResponseConstants.Default,
                SlideButtonActionType.InputCompleted => SlideButtonActionTypeResponseConstants.InputCompleted,
                SlideButtonActionType.InputMissed => SlideButtonActionTypeResponseConstants.InputMissed,
                _ => throw new System.NotImplementedException(),
            };

            return new SlideButtonActionResponse(
                type,
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
