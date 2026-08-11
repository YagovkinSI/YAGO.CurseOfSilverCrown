using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Dataset.Prologue;
using YAGO.World.Domain.Services;

namespace YAGO.World.Domain.Colonies
{
    /// <summary>
    /// Колония
    /// </summary>
    public class Colony : IEntity<Guid>
    {
        /// <summary>
        /// Идентифиикатор колонии
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентифиикатор пользователя владельца
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Название
        /// </summary>
        public ColonyName Name { get; private set; }

        /// <summary>
        /// Параметры колонии
        /// </summary>
        public ColonyState State { get; }

        /// <summary>
        /// Квесты колонии
        /// </summary>
        public IReadOnlyList<ColonyEvent> Events { get; private set; }

        public Colony(
            Guid id,
            long userId,
            ColonyName name,
            ColonyState stats,
            IReadOnlyList<ColonyEvent> events)
        {
            Id = id;
            UserId = userId;
            Name = name;
            State = stats;
            Events = events;
        }

        public static IReadOnlyList<IEntity> CreateNew(long userId)
        {
            var name = ColonyName.CreateNew();
            var colonyStats = ColonyState.CreateNew();
            var startEvent = ColonyEvent.CreateNew(nameof(ColonyNameEvent));
            var colony = new Colony(
                id: Guid.NewGuid(),
                userId: userId,
                name: name,
                colonyStats,
                events: [startEvent]);
            var cycle = Cycle.CreateNew(
                colony.Id,
                prevCycle: null);
            return [colony, cycle];
        }

        public void SetName(string name)
        {
            Name.SetName(name);
        }

        public bool IsNewColonyAvailable()
        {
            return Events.Count == 0;
        }

        public void RemoveEvent(string id)
        {
            var list = Events.ToList();
            var removingQuest = list.Single(x => x.EventId == id);
            list.Remove(removingQuest);
            Events = list;
        }

        public void AddEvents(IReadOnlyList<string> newEvents)
        {
            var list = new List<ColonyEvent>(newEvents.Count);
            foreach (var eventId in newEvents)
            {
                if (Events.Any(x => x.EventId == eventId))
                    continue;
                var colonyEvent = ColonyEvent.CreateNew(eventId);
                list.Add(colonyEvent);
            }

            Events = [
                ..Events,
                ..list];
        }

        public void SetChanges(GameEventChangeList changeList)
        {
            State.SetEpisodeParameters(changeList.ColonyStats);
            AddEvents(changeList.NewQuests);
        }
    }
}