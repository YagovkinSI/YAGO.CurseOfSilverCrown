using System.Linq;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class EpisodeResponseMapping
    {
        public static EpisodeResponse ToResponse(this Episode source)
        {
            return new EpisodeResponse(
                source.Id,
                source.Slides.Select(x => x.ToResponse()).ToList(),
                source.ChoiceLabel,
                source.Choice?.Select(x => x.ToResponse()).ToList());
        }

        private static SlideResponse ToResponse(this Slide source)
        {
            return new SlideResponse(
                source.Title,
                source.Illustration,
                source.Text,
                source.Parameters);
        }
    }
}
