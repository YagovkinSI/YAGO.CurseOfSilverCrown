using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Exceptions;

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
                source.Episode.Title,
                source.Episode.PrologueSlides.Select(x => x.ToResponse()).ToList(),
                dilemma,
                IsCycleCompleted);
        }

        private static DilemmaResponse? ToResponse(this Dilemma source, IReadOnlyList<ColonyChoice> colonyChoices)
        {
            return source switch
            {
                DilemmaSelect dilemmaSelect => new DilemmaSelectResponse(
                    colonyChoices.Select(x => x.ToResponse()).ToList(),
                    dilemmaSelect.ChoiceLabel),
                DilemmaTextInput dilemmaTextInput => new DilemmaTextInputResponse(
                    dilemmaTextInput.Slide.ToResponse(),
                    dilemmaTextInput.SubmitButtonName),
                _ => throw new YagoUnknownTypeException(source.GetType().Name)
            };
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

        private static SlideResponse ToResponse(this Slide source)
        {
            return new SlideResponse(
                source.Title,
                source.ImageName,
                source.Text,
                source.Parameters);
        }
    }
}
