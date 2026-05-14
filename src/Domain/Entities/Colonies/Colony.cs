using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.Quests;

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
        public IReadOnlyList<ColonyQuest> Quests { get; }

        /// <summary>
        /// Флаг деактивации колонии игроком
        /// </summary>
        public bool Deactivated { get; private set; }

        /// <summary>
        /// Время деактивации колонии игроком
        /// </summary>
        public DateTime? DeactivateAtUtc { get; private set; }

        public bool HasName => Stats.EpisodeCount > 0;

        public Colony(
            Guid id,
            long userId,
            string name,
            ColonyStats stats,
            IReadOnlyList<ColonyQuest> quests,
            bool deactivated,
            DateTime? deactivateAtUtc)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Stats = stats;
            Quests = quests;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
        }

        public static IReadOnlyList<IEntity> CreateNew(long userId)
        {
            var random = new Random();
            var name = $"Колония {random.Next(100000, 999999)}";

            var colonyStats = ColonyStats.CreateNew();
            var colonyQuests = GetStartQuests(colonyStats);
            var colony = new Colony(
                id: Guid.NewGuid(),
                userId: userId,
                name: name,
                colonyStats,
                colonyQuests,
                deactivated: false,
                deactivateAtUtc: null);
            var cycle = Cycle.CreateNew(
                colony.Id,
                prevCycle: null);
            return [colony, cycle];
        }

        private static List<ColonyQuest> GetStartQuests(ColonyStats colonyStats)
        {
            return
            [
                new(colonyStats, QuestDataset.Get(Guid.Parse("00000000-0000-0000-0000-000000000001"))),
                new(colonyStats, QuestDataset.Get(Guid.Parse("00000000-0000-0000-0000-000000000002"))),
                new(colonyStats, QuestDataset.Get(Guid.Parse("00000000-0000-0000-0000-000000000003")))
            ];
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
            return Stats.ZonesOccupied > 130;
        }
    }
}