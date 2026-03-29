using System;
using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Колония
    /// </summary>
    public class Colony : IEntity
    {
        /// <summary>
        /// Идентифиикатор колонии
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентифиикатор пользователя владельца
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Параметры колонии
        /// </summary>
        public ColonyStats Stats { get; }

        /// <summary>
        /// Флаг деактивации колонии игроком
        /// </summary>
        public bool Deactivated { get; private set; }

        /// <summary>
        /// Время деактивации колонии игроком
        /// </summary>
        public DateTime? DeactivateAtUtc { get; private set; }

        public Colony(
            long id,
            long userId,
            string name,
            ColonyStats stats,
            bool deactivated,
            DateTime? deactivateAtUtc)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Stats = stats;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
        }

        public static Colony CreateNew(
            long userId,
            string name,
            CodeOfLaws gavernorType)
        {
            var colonyStats = ColonyStats.CreateNew(gavernorType);
            return new Colony(
                id: default,
                userId: userId,
                name: name,
                colonyStats,
                deactivated: false,
                deactivateAtUtc: null);
        }

        public void Deactivate()
        {
            Deactivated = true;
            DeactivateAtUtc = DateTime.UtcNow;
        }
    }
}