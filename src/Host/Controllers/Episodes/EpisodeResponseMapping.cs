using System.Linq;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class EpisodeResponseMapping
    {
        public static EpisodeResponse ToResponse(this Episode source, bool IsCycleCompleted)
        {
            return new EpisodeResponse(
                source.Id,
                source.PrologSlides.Select(x => x.ToResponse()).ToList(),
                source.ChoiceSlides.Select(x => x.ToResponse()).ToList(),
                source.ChoiceLabel,
                IsCycleCompleted);
        }

        private static SlideResponse ToResponse(this Slide source)
        {
            return new SlideResponse(
                source.Id,
                source.Title,
                source.ImageName,
                source.Text,
                source.Parameters);
        }
    }
}
