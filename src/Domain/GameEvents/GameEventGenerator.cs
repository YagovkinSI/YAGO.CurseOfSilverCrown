using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameEvents
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
                .Where(gameEvent => gameEvent.StartOptions.Check(colony.State))
                .ToList();
            return new GameEventGenerateResult(episodes);
        }        
    }

    public record GameEventGenerateResult(IReadOnlyList<GameEvent> Events);
}
