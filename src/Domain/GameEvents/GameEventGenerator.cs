using System;
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
                .Where(gameEvent => Check(gameEvent, colony))
                .ToList();
            return new GameEventGenerateResult(episodes);
        }

        private bool Check(GameEvent gameEvent, Colony colony)
        {
            var finalChance = gameEvent.StartOptions.ChanceCalculate(colony);

            switch (finalChance)
            {
                case <= 0:
                    return false;
                case >= 1:
                    return true;
                default:
                    var randomResult = new Random().NextDouble();
                    return randomResult < finalChance;
            }
        }
    }

    public record GameEventGenerateResult(IReadOnlyList<GameEvent> Events);
}
