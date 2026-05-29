using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using System.Linq;

namespace YAGO.World.Domain.Services
{
    public interface IGameEventGenerator
    {
        GameEventGenerateResult Generate(IReadOnlyList<GameEvent> gameEvents, Colony colony);
    }

    public class GameEventGenerator : IGameEventGenerator
    {
        public GameEventGenerateResult Generate(IReadOnlyList<GameEvent> gameEvents, Colony colony)
        {
            var episodes = gameEvents
                .Where(gameEvent => gameEvent.Check(colony))
                .Select(gameEvent => gameEvent.Episode)
                .ToList();

            var endingEpisode = GetCycleEndingEpisode(colony);

            episodes.Add(endingEpisode);

            return new GameEventGenerateResult(episodes);
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
            return new Episode(slides: [slide], changesWithoutChoice: colonyParameters);
        }
    }

    public record GameEventGenerateResult(IReadOnlyList<Episode> Episodes);
}
