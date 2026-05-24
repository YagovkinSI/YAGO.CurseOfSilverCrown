using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Services
{
    public interface IGameEventGenerator
    {
        GameEventGenerateResult Generate(IReadOnlyList<GameEvent> gameEvents, int startStepNumber, Colony colony);
    }

    public class GameEventGenerator : IGameEventGenerator
    {
        public GameEventGenerateResult Generate(IReadOnlyList<GameEvent> gameEvents, int startStepNumber, Colony colony)
        {
            for (var i = startStepNumber; i < gameEvents.Count; i++)
            {
                var gameEvent = gameEvents[i];
                if (gameEvent.Check(colony))
                {
                    return new GameEventGenerateResult(
                        gameEvent.Id,
                        gameEvent.Episode,
                        StepNumber: i + 1,
                        IsCycleEnded: false,
                        DaysPassedOptions: gameEvent.AdditionalDaysPassed ?? new DaysPassedOptions(0));
                }
            }

            var episode = GetCycleEndingEpisode(colony);
            return new GameEventGenerateResult(
                "NextCycle",
                episode,
                StepNumber: gameEvents.Count,
                IsCycleEnded: true,
                DaysPassedOptions: new DaysPassedOptions(0));
        }

        private static Episode GetCycleEndingEpisode(Colony colony)
        {
            var colonyStats = colony.Stats;
            var colonyParameters = new List<KeyValueParameter>()
            {
                new(ColonyStatNames.ActionPoints_Resourses, colonyStats.GetGameParameter(ColonyStatNames.ActionPoints_Trend)),
                new(ColonyStatNames.Economic_Reserves, colonyStats.GetGameParameter(ColonyStatNames.Economic_Budget_Balance)),
                new(ColonyStatNames.Mood_Total, colonyStats.GetGameParameter(ColonyStatNames.Mood_Total_Balance))
            };
            var slide = new Slide(
                id: "TurnIsOver_0",
                "Успешное завершение цикла",
                ImageSet.RegularCycle,
                new string[]
                {
                    "В трюмах ритмично гудят дробилки, на мостике горят зелёные лампочки систем. " +
                    "Рудокопы в своих сменах монотонно, но эффективно откалывают породу.",
                    "Цикл успешно завершен, прибыль получена.",
                },
                colonyParameters,
                continueButtonName: "Далее",
                buttons: []);
            return new Episode(slides: [slide], dilemma: null);
        }
    }

    public record GameEventGenerateResult(string EventId, Episode Episode, int StepNumber, bool IsCycleEnded, DaysPassedOptions DaysPassedOptions);
}
