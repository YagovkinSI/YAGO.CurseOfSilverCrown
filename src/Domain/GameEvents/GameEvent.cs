using System.Collections.Generic;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Domain.GameEvents
{
    public class GameEvent
    {
        public string Code { get; }
        public EventType Type { get; }
        public GameActionChance StartOptions { get; }
        public IReadOnlyList<Slide> Slides { get; }
        public Dictionary<string, GameAction> Actions { get; }
        public IReadOnlyList<string> Tags { get; }

        public GameEvent(
            string code,
            EventType eventType,
            GameActionChance eventOccurrenceOptions,
            IEnumerable<Slide> slides,
            Dictionary<string, GameAction>? actions = null,
            IReadOnlyList<string>? tags = null)
        {
            Code = code;
            Type = eventType;
            StartOptions = eventOccurrenceOptions;
            Slides = [.. slides];
            Actions = actions ?? [];
            Tags = tags ?? [];
        }
    }
}
