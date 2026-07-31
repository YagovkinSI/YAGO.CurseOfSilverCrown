using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue;
using YAGO.World.Domain.Services;

namespace YAGO.World.Domain.Entities.Colonies
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

        /// <summary>
        /// Флаг деактивации колонии игроком
        /// </summary>
        public bool Deactivated { get; private set; }

        /// <summary>
        /// Время деактивации колонии игроком
        /// </summary>
        public DateTime? DeactivateAtUtc { get; private set; }

        public Colony(
            Guid id,
            long userId,
            ColonyName name,
            ColonyState stats,
            IReadOnlyList<ColonyEvent> events,
            bool deactivated,
            DateTime? deactivateAtUtc)
        {
            Id = id;
            UserId = userId;
            Name = name;
            State = stats;
            Events = events;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
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
                events: [startEvent],
                deactivated: false,
                deactivateAtUtc: null);
            var cycle = Cycle.CreateNew(
                colony.Id,
                prevCycle: null);
            return [colony, cycle];
        }

        public void Deactivate()
        {
            Deactivated = true;
            DeactivateAtUtc = DateTime.UtcNow;
        }

        public void SetName(string name) => Name.SetName(name);

        public bool IsNewColonyAvailable()
        {
            return Events.Count == 0;
        }

        public void RemoveEvent(string id)
        {
            var list = Events.ToList();
            var removingQuest = list.First(x => x.EventId == id);
            list.Remove(removingQuest);
            Events = list;
        }

        public void AddEvents(IReadOnlyList<string> newEvents)
        {
            if (!newEvents.Any())
                return;

            var colonyEvents = newEvents.Select(x => ColonyEvent.CreateNew(x));
            var list = Events.ToList();
            list.AddRange(colonyEvents);
            Events = [.. list];
        }

        public void SetChanges(GameEventChangeList changeList)
        {
            State.SetEpisodeParameters(changeList.ColonyStats);
            AddEvents(changeList.NewQuests);
        }
    }
}