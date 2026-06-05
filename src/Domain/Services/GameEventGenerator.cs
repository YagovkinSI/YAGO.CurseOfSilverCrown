using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
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
                .Where(gameEvent => gameEvent.EventOccurrenceOptions.Check(colony.Stats))
                .ToList();

            var cycleEndingChangeList = GetCycleEndingChangeList(colony);

            return new GameEventGenerateResult(episodes, cycleEndingChangeList);
        }

        private static GameEventChangeList GetCycleEndingChangeList(Colony colony)
        {
            var colonyStats = colony.Stats;
            var colonyParameters = new List<KeyValueParameter>()
            {
                new(ColonyStatNames.ActionPoints_Resourses, colonyStats.GetGameParameter(ColonyStatNames.ActionPoints_Trend)),
                new(ColonyStatNames.Economic_Reserves, colonyStats.GetGameParameter(ColonyStatNames.Economic_Budget_Balance)),
                new(ColonyStatNames.Mood_Total, colonyStats.GetGameParameter(ColonyStatNames.Mood_Total_Balance)),
                new(ColonyStatNames.CurrentWeek, 1)
            };
            return new GameEventChangeList(colonyParameters, newQuests: []);
        }
    }

    public record GameEventGenerateResult(IReadOnlyList<GameEvent> Events, GameEventChangeList CycleEndingChangeList);
}
