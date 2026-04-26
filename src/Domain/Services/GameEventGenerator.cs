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
                    return new GameEventGenerateResult(
                        gameEvent.Episode,
                        StepNumber: i + 1,
                        IsCycleEnded: false,
                        DaysPassed: 0 + gameEvent.AdditionalDaysPassed);
            }

            var episode = GetCycleEndingEpisode(colony);
            return new GameEventGenerateResult(
                episode,
                StepNumber: gameEvents.Count,
                IsCycleEnded: true,
                DaysPassed: 0);
        }

        private Episode GetCycleEndingEpisode(Colony colony)
        {
            var colonyStats = colony.Stats;
            var colonyParameters = new List<KeyValueParameter>()
            {
                new(ColonyStatNames.Economic_Reserves, colonyStats.GetGameParameter(ColonyStatNames.Economic_Budget_Balance)),
                new(ColonyStatNames.Mood_Total, colonyStats.GetGameParameter(ColonyStatNames.Mood_Total_Balance))
            };
            var slide = new PrologueSlide(
                "Успешное завершение цикла",
                ImageSet.RegularCycle,
                new string[]
                {
                    "В трюмах ритмично гудят дробилки, на мостике горят зелёные лампочки систем. " +
                    "Рудокопы в своих сменах монотонно, но эффективно откалывают породу.",
                    "Цикл успешно завершен, прибыль получена.",
                },
                colonyParameters,
                continueButtonName: "Далее");
            return new Episode(id: null, title: "Успешное завершение цикла", prologSlides: [slide], dilemma: null);
        }
    }

    public record GameEventGenerateResult(Episode Episode, int StepNumber, bool IsCycleEnded, int DaysPassed);
}
