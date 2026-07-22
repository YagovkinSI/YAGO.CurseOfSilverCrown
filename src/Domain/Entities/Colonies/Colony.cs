using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue;

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
        public IReadOnlyList<string> EventIds { get; private set; }

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
            IReadOnlyList<string> eventIds,
            bool deactivated,
            DateTime? deactivateAtUtc)
        {
            Id = id;
            UserId = userId;
            Name = name;
            State = stats;
            EventIds = eventIds;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
        }

        public static IReadOnlyList<IEntity> CreateNew(long userId)
        {
            var name = ColonyName.CreateNew();
            var colonyStats = ColonyState.CreateNew();
            var colony = new Colony(
                id: Guid.NewGuid(),
                userId: userId,
                name: name,
                colonyStats,
                eventIds: [nameof(ColonyNameEvent)],
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
            return EventIds.Count == 0;
        }

        public void RemoveEvent(string id)
        {
            var list = EventIds.ToList();
            var removingQuest = list.First(x => x == id);
            list.Remove(removingQuest);
            EventIds = list;
        }

        public void AddEvents(IReadOnlyList<string> newEvents)
        {
            if (!newEvents.Any())
                return;

            var list = EventIds.ToList();
            list.AddRange(newEvents);
            EventIds = list.Distinct().ToList();
        }

        public void SetChanges(GameEventChangeList changeList)
        {
            State.SetEpisodeParameters(changeList.ColonyStats);
            AddEvents(changeList.NewQuests);
        }        
    }
}