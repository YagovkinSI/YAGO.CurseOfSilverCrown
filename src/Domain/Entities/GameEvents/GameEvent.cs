using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.Quests;
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

        public GameEvent(
            string id,
            EventOccurrenceOptions eventOccurrenceOptions,
            Episode episode,
            Dictionary<string, GameEventChangeList>? changeList = null,
            bool isImmediatelyEvent = false)
        {
            Id = id;
            EventOccurrenceOptions = eventOccurrenceOptions;
            Episode = episode;
            ChangeList = changeList ?? [];
            IsImmediatelyEvent = isImmediatelyEvent;
        }

        public (QuestType QuestType, string Progress) GetQuestTypeAndProgress(ColonyStats colonyStats)
        {
            if (IsImmediatelyEvent)
                return (QuestType.Immediately, "Завершить");

            var actions = Episode.Slides
                .SelectMany(x => x.Buttons)
                .Where(x => x.Action != null);
            var actionCount = actions.Count();

            if (actionCount == 0)
                return (QuestType.News, "Событие");
            else if (actionCount == 1)
            {
                var requirements = actions.Single().AvailableRequirements;
                var completed = requirements.Count(requirement => requirement.Parameter.Check(colonyStats));
                var type = completed == requirements.Count ? QuestType.Ready : QuestType.Default;
                var progress = completed == requirements.Count ? "Завершить" : $"{completed}/{requirements.Count}";
                return (type, progress);
            }
            else
            {
                var completed = actions.Count(x => x.AvailableRequirements.Check(colonyStats).IsAvailable);
                var type = completed == actionCount ? QuestType.Ready : QuestType.Default;
                var progress = $"Выбор {completed}/{actionCount}";
                return (type, progress);
            }
        }
    }
}
