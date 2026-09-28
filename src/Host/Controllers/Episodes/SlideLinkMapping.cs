using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class SlideLinkMapping
    {
        public static SlideLinkResponse ToResponse(this SlideLink source)
        {
            return source.Type switch
            {
                SlideLinkType.Rating => new SlideLinkResponse(
                    Label: "Рейтинг игроков",
                    Url: "/rating"),
            };
        }
    }
}