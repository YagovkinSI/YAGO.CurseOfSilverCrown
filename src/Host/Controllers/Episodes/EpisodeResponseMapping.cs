using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
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
                [.. source.Episode.PrologueSlides.Select(x => x.ToResponse(isChange: true))],
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

        public static PrologueSlideResponse ToResponse(this PrologueSlide source, bool isChange)
        {
            var colonyParameters = GetColonyParameters(source.Parameters, isChange);

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

        private static IReadOnlyList<ColonyParameterResponse> GetColonyParameters(IReadOnlyList<KeyValueParameter> source, bool isChange = true)
        {
            var result = new List<ColonyParameterResponse>(source.Count);

            foreach (var item in source)
            {
                var colonyParameter = item.Name switch
                {
                    ColonyStatNames.ActionPoints_Resourses => ColonyParameterResponse.ActionPoints_Resourses((int)item.Value, isChange),
                    ColonyStatNames.ActionPoints_Trend => ColonyParameterResponse.ActionPoints_Trend((int)item.Value, isChange),
                    ColonyStatNames.Economic_Reserves => ColonyParameterResponse.FinanceReserves(item.Value, isChange),
                    ColonyStatNames.Economic_Budget_Balance => ColonyParameterResponse.FinanceTrend(item.Value, isChange),
                    ColonyStatNames.Mood_Total => ColonyParameterResponse.TrustResourse(item.Value, isChange),
                    ColonyStatNames.AreaCapacity_Occupied => ColonyParameterResponse.AreaResourse((int)-item.Value, isChange),
                    ColonyStatNames.Population_Total => ColonyParameterResponse.Population((int)item.Value, isChange),
                    _ => null,
                };
                if (colonyParameter != null)
                    result.Add(colonyParameter);
            }

            return result;
        }
    }
}
