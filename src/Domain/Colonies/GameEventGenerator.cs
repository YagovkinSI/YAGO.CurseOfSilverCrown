using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Domain.Colonies
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

            var turnEndingChangeList = GetTurnEndingChangeList(colony);

            return new GameEventGenerateResult(episodes, turnEndingChangeList);
        }

        private static GameEventChangeList GetTurnEndingChangeList(Colony colony)
        {
            var colonyStats = colony.State;
            var colonyParameters = new List<KeyValueParameter>()
            {
                new(StateKey.ActionPointsCurrent, colonyStats.GetValue(StateKey.ActionPointsDelta)),
                new(StateKey.SolarsCurrent, colonyStats.GetValue(StateKey.SolarsDelta)),
                new(StateKey.MoodCurrent, colonyStats.GetValue(StateKey.MoodDelta)),
                new(StateKey.TurnsCurrent, 1)
            };
            return new GameEventChangeList(colonyParameters, newQuests: []);
        }
    }

    public record GameEventGenerateResult(IReadOnlyList<GameEvent> Events, GameEventChangeList TurnEndingChangeList);
}
