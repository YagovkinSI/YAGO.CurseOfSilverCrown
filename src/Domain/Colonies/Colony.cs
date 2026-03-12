using System;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Colonies
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
            ColonyStats colonyStats,
            bool deactivated,
            DateTime? deactivateAtUtc)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Stats = colonyStats;
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

        public void AddSolars(double value)
        {
            Stats.AddSolars(value);
        }

        public void AddCompany(long companyId)
        {
            Stats.AddCompany(companyId);
        }

        public void SetShip(int shipId)
        {
            Stats.SetShip(shipId);
        }

        public void Deactivate()
        {
            Deactivated = true;
            DeactivateAtUtc = DateTime.UtcNow;
        }

        public void ValidateShip(Ship ship)
        {
            Stats.ValidateShip(ship);
        }

        public void ValidateContracts(ColonyCompanies companies)
        {
            Stats.ValidateContracts(companies);
        }

        public void AddFestivalEffect(double effect)
        {
            Stats.AddFestivalEffect(effect);
        }

        internal void AddWeek()
        {
            Stats.AddWeek();
        }

        internal void SetFirstWedding()
        {
            Stats.SetFirstWedding();
        }
    }
}
