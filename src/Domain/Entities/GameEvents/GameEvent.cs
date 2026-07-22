using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
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
        public Dictionary<string, GameEventChangeList> ChangeList { get; }
        public Episode Episode { get; }
        public bool IsImmediatelyEvent { get; }
        public bool IsAutostartEvent { get; }
        public Dictionary<string, EventResult> Results { get; }

        public GameEvent(
            string id,
            EventOccurrenceOptions eventOccurrenceOptions,
            Episode episode,
            Dictionary<string, GameEventChangeList>? changeList = null,
            bool isImmediatelyEvent = false,
            bool isAutostartEvent = false,
            Dictionary<string, EventResult>? results = null)
        {
            Id = id;
            EventOccurrenceOptions = eventOccurrenceOptions;
            Episode = episode;
            ChangeList = changeList ?? [];
            IsImmediatelyEvent = isImmediatelyEvent;
            IsAutostartEvent = isAutostartEvent;
            Results = results ?? [];
        }

        public (EventType EventType, string Progress) GetQuestTypeAndProgress(ColonyStates colonyStats)
        {
            if (IsAutostartEvent)
                return (EventType.Autostart, "Завершить");

            if (IsImmediatelyEvent)
                return (EventType.Immediately, "Завершить");

            var actions = Episode.Slides
                .SelectMany(x => x.Buttons)
                .Where(x => x.Action != null);
            var actionCount = actions.Count();

            if (actionCount == 0)
                return (EventType.News, "Событие");
            else if (actionCount == 1)
            {
                var requirements = actions.Single().Requirements;
                if (requirements.Count == 0 && Episode.Slides.All(x => x.TextInput == null))
                    return (EventType.News, "Событие");
                var completed = requirements.Count(requirement => requirement.Check(colonyStats));
                var type = completed == requirements.Count ? EventType.Ready : EventType.Default;
                var progress = completed == requirements.Count ? "Завершить" : $"{completed}/{requirements.Count}";
                return (type, progress);
            }
            else
            {
                var completed = actions.Count(x => x.Requirements.All(x => x.Check(colonyStats)));
                var type = completed == actionCount ? EventType.Ready : EventType.Default;
                var progress = $"Выбор {completed}/{actionCount}";
                return (type, progress);
            }
        }
    }
}
