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
        public Dictionary<string, GameAction> ChangeList { get; }
        public Dictionary<string, GameActionResult> Results { get; }

        public GameEvent(
            string code,
            EventType eventType,
            GameActionChance eventOccurrenceOptions,
            IEnumerable<Slide> slides,
            Dictionary<string, GameAction>? changeList = null,
            Dictionary<string, GameActionResult>? results = null)
        {
            Code = code;
            Type = eventType;
            StartOptions = eventOccurrenceOptions;
            Slides = [.. slides];
            ChangeList = changeList ?? [];
            Results = results ?? [];
        }
    }
}
