using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;

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

            var colonyParameters = GetColonyParameters(source.Choice.Parameters);

            return new ChoiceResponse(
                source.Choice.Id,
                source.Choice.Title,
                source.Choice.ImageName,
                source.Choice.Text,
                colonyParameters,
                isAvailable,
                buttonName);
        }

        private static PrologueSlideResponse ToResponse(this PrologueSlide source)
        {
            var colonyParameters = GetColonyParameters(source.Parameters);

            return new PrologueSlideResponse(
                source.Title,
                source.ImageName,
                source.Text,
                colonyParameters,
                source.ContinueButtonName);
        }

        private static SlideResponse ToResponse(this Slide source)
        {
            var colonyParameters = GetColonyParameters(source.Parameters);

            return new SlideResponse(
                source.Title,
                source.ImageName,
                source.Text,
                colonyParameters);
        }

        private static IReadOnlyList<ColonyParameterResponse> GetColonyParameters(IReadOnlyList<KeyValueParameter> source)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                var colonyParameter = item.Name switch
                {
                    ColonyParameterNames.Economic_Reserves => ColonyParameterResponseMapping.EconomicReserves(item.Value, isChange: true),
                    ColonyParameterNames.Economic_Budget_Balance => ColonyParameterResponseMapping.EconomicBudgetBalance(item.Value, isChange: true),
                    ColonyParameterNames.Mood_Total => ColonyParameterResponseMapping.MoodTotal(item.Value, isChange: true),
                    ColonyParameterNames.AreaCapacity_Occupied => ColonyParameterResponseMapping.AreaCapacityOccupied((int)-item.Value, isChange: true),
                    ColonyParameterNames.Population_Total => ColonyParameterResponseMapping.GetPopulation((int)item.Value, isChange: true),
                    _ => null,
                };
                if (colonyParameter != null)
                    result.Add(colonyParameter);
            }

            return result;
        }
    }
}
