using System.Collections.Generic;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public class GameEvent
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; }
        public EventOccurrenceOptions EventOccurrenceOptions { get; }
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice { get; }
        public Episode Episode { get; }

        public GameEvent(
            string id,
            EventOccurrenceOptions eventOccurrenceOptions,
            Episode episode,
            IReadOnlyList<KeyValueParameter>? changesWithoutChoice = null)
        {
            Id = id;
            EventOccurrenceOptions = eventOccurrenceOptions;
            Episode = episode;
            ChangesWithoutChoice = changesWithoutChoice;
        }
    }
}
