using System.Linq;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Host.Controllers.Episodes
{
    public static class EpisodeResponseMapping
    {
        public static EpisodeResponse ToResponse(this ColonyEpisode source, bool IsCycleCompleted)
        {
            var choises = source.GetColonyChoices();

            return new EpisodeResponse(
                source.Episode.Id,
                source.Episode.PrologSlides.Select(x => x.ToResponse()).ToList(),
                choises.Select(x => x.ToResponse()).ToList(),
                source.Episode.ChoiceType.ToString(),
                source.Episode.ChoiceLabel,
                IsCycleCompleted);
        }

        private static ChoiceResponse ToResponse(this ColonyChoice source)
        {
            var (isAvailable, buttonName) = source.CheckAvailability();

            return new ChoiceResponse(
                source.Choice.Id,
                source.Choice.Title,
                source.Choice.ImageName,
                source.Choice.Text,
                source.Choice.Parameters,
                isAvailable,
                buttonName);
        }

        private static SlideResponse ToResponse(this Slide source)
        {
            return new SlideResponse(
                source.Title,
                source.ImageName,
                source.Text,
                source.Parameters,
                source.ButtonName);
        }
    }
}
