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
                .ToList();

            var endingEpisode = GetCycleEndingEpisode(colony);

            episodes.Add(endingEpisode);

            return new GameEventGenerateResult(episodes);
        }

        private static GameEvent GetCycleEndingEpisode(Colony colony)
        {
            var id = "TurnIsOver";
            var colonyStats = colony.Stats;
            var colonyParameters = new List<KeyValueParameter>()
            {
                new(ColonyStatNames.ActionPoints_Resourses, colonyStats.GetGameParameter(ColonyStatNames.ActionPoints_Trend)),
                new(ColonyStatNames.Economic_Reserves, colonyStats.GetGameParameter(ColonyStatNames.Economic_Budget_Balance)),
                new(ColonyStatNames.Mood_Total, colonyStats.GetGameParameter(ColonyStatNames.Mood_Total_Balance))
            };
            var slide = new Slide(
                id: $"{id}_0",
                "Успешное завершение цикла",
                ImageSet.RegularCycle,
                new string[]
                {
                    "В трюмах ритмично гудят дробилки, на мостике горят зелёные лампочки систем. " +
                    "Рудокопы в своих сменах монотонно, но эффективно откалывают породу.",
                    "Цикл успешно завершен, прибыль получена.",
                },
                colonyParameters,
                buttons: []);
            var episode = new Episode(slides: [slide]);
            return new GameEvent(id, 1, [], [], episode, changesWithoutChoice: colonyParameters);
        }
    }

    public record GameEventGenerateResult(IReadOnlyList<GameEvent> Events);
}
