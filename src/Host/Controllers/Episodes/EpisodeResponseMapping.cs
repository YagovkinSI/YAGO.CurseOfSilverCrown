using System.Collections.Generic;
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
            var dilemma = source.Episode.Dilemma?.ToResponse(choises);
            return new EpisodeResponse(
                source.Episode.Id,
                source.Episode.PrologueSlides.Select(x => x.ToResponse()).ToList(),
                dilemma,
                IsCycleCompleted);
        }

        private static DilemmaResponse ToResponse(this Dilemma source, IReadOnlyList<ColonyChoice> colonyChoices)
        {
            return new DilemmaResponse(
                colonyChoices.Select(x => x.ToResponse()).ToList(),
                source.ChoiceType.ToString(),
                source.ChoiceLabel);
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

        private static PrologueSlideResponse ToResponse(this PrologueSlide source)
        {
            return new PrologueSlideResponse(
                source.Title,
                source.ImageName,
                source.Text,
                source.Parameters,
                source.ContinueButtonName);
        }
    }
}
