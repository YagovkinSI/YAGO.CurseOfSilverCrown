using System.Collections.Generic;
using YAGO.World.Domain.Episodes;

namespace YAGO.World.Domain.GameEvents
{
    public class GameEvent
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; }
        public EventType EventType { get; }
        public EventOccurrenceOptions EventOccurrenceOptions { get; }
        public Dictionary<string, GameEventChangeList> ChangeList { get; }
        public Episode Episode { get; }
        public Dictionary<string, EventResult> Results { get; }

        public GameEvent(
            string id,
            EventType eventType,
            EventOccurrenceOptions eventOccurrenceOptions,
            Episode episode,
            Dictionary<string, GameEventChangeList>? changeList = null,
            Dictionary<string, EventResult>? results = null)
        {
            Id = id;
            EventType = eventType;
            EventOccurrenceOptions = eventOccurrenceOptions;
            Episode = episode;
            ChangeList = changeList ?? [];
            Results = results ?? [];
        }
    }
}
