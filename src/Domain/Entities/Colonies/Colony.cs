using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Cycles;
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
        public string Name { get; private set; }

        /// <summary>
        /// Параметры колонии
        /// </summary>
        public ColonyStats Stats { get; }

        /// <summary>
        /// Квесты колонии
        /// </summary>
        public IReadOnlyList<string> QuestIds { get; private set; }

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
            string name,
            ColonyStats stats,
            IReadOnlyList<string> questIds,
            bool deactivated,
            DateTime? deactivateAtUtc)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Stats = stats;
            QuestIds = questIds;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
        }

        public static IReadOnlyList<IEntity> CreateNew(long userId)
        {
            var random = new Random();
            var name = $"Колония {random.Next(100000, 999999)}";

            var colonyStats = ColonyStats.CreateNew();
            var colony = new Colony(
                id: Guid.NewGuid(),
                userId: userId,
                name: name,
                colonyStats,
                questIds: [nameof(ColonyNameEvent)],
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

        public void SetName(string name)
        {
            Name = name;
        }

        public bool IsAutoRunCycle()
        {
            return Stats.EpisodeCount == 0;
        }

        public bool IsNewColonyAvailable()
        {
            return Stats.EpisodeCount > 20 && QuestIds.Count == 0;
        }

        public void RemoveQuest(string id)
        {
            var list = QuestIds.ToList();
            var removingQuest = list.Single(x => x == id);
            list.Remove(removingQuest);
            QuestIds = list;
        }

        public void UpdateQuests(IReadOnlyList<string> newQuestIds)
        {
            var list = QuestIds.ToList();
            list.AddRange(newQuestIds);
            QuestIds = list;
        }
    }
}