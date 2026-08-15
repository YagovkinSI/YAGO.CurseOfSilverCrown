using System.Collections.Generic;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Domain.GameEvents
{
    public class GameEvent
    {
        public string Code { get; }
        public EventType Type { get; }
        public EventOccurrenceOptions EventOccurrenceOptions { get; }
        public Episode Episode { get; }
        public Dictionary<string, GameEventChangeList> ChangeList { get; }
        public Dictionary<string, EventResult> Results { get; }

        public GameEvent(
            string code,
            EventType eventType,
            EventOccurrenceOptions eventOccurrenceOptions,
            Episode episode,
            Dictionary<string, GameEventChangeList>? changeList = null,
            Dictionary<string, EventResult>? results = null)
        {
            Code = code;
            Type = eventType;
            EventOccurrenceOptions = eventOccurrenceOptions;
            Episode = episode;
            ChangeList = changeList ?? [];
            Results = results ?? [];
        }
    }
}
