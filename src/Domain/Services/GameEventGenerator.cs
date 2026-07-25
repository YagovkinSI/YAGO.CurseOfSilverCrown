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
                .Where(gameEvent => gameEvent.EventOccurrenceOptions.Check(colony.State))
                .ToList();

            var cycleEndingChangeList = GetCycleEndingChangeList(colony);

            return new GameEventGenerateResult(episodes, cycleEndingChangeList);
        }

        private static GameEventChangeList GetCycleEndingChangeList(Colony colony)
        {
            var colonyStats = colony.State;
            var colonyParameters = new List<KeyValueParameter>()
            {
                new(StateKey.ReformPointsCurrent, colonyStats.GetGameParameter(StateKey.ReformPointsDelta)),
                new(StateKey.SolarsCurrent, colonyStats.GetGameParameter(StateKey.SolarsDelta)),
                new(StateKey.MoodCurrent, colonyStats.GetGameParameter(StateKey.MoodDelta)),
                new(StateKey.TurnsCurrent, 1)
            };
            return new GameEventChangeList(colonyParameters, newQuests: []);
        }
    }

    public record GameEventGenerateResult(IReadOnlyList<GameEvent> Events, GameEventChangeList CycleEndingChangeList);
}
