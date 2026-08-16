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
            return new GameEventGenerateResult(episodes);
        }        
    }

    public record GameEventGenerateResult(IReadOnlyList<GameEvent> Events);
}
