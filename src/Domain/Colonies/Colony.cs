using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Exceptions;

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
        /// была ли первая свадьба
        /// </summary>
        public bool FirstWedding { get; private set; }

        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId { get; private set; }

        /// <summary>
        /// Установленные законы
        /// </summary>
        public CodeOfLaws CodeOfLaws { get; }

        /// <summary>
        /// Флаг деактивации колонии игроком
        /// </summary>
        public bool Deactivated { get; private set; }

        /// <summary>
        /// Время деактивации колонии игроком
        /// </summary>
        public DateTime? DeactivateAtUtc { get; private set; }

        /// <summary>
        /// Пройденные эпизоды
        /// </summary>
        public Dictionary<long, string> Episodes { get; private set; }

        public Colony(
            long id,
            long userId,
            string name,
            ColonyStats colonyStats,
            bool firstWedding,
            long shipId,
            CodeOfLaws startGavernorType,
            bool deactivated,
            DateTime? deactivateAtUtc,
            Dictionary<long, string> episodes)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Stats = colonyStats;
            FirstWedding = firstWedding;
            ShipId = shipId;
            CodeOfLaws = startGavernorType;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
            Episodes = episodes;
        }

        public static Colony CreateNew(
            long userId,
            string name,
            CodeOfLaws gavernorType)
        {
            var colonyStats = ColonyStats.CreateNew();

            return new Colony(
                id: default,
                userId: userId,
                name: name,
                colonyStats,
                firstWedding: false,
                shipId: 1,
                startGavernorType: gavernorType,
                deactivated: false,
                deactivateAtUtc: null,
                episodes: []
            );
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
            ShipId = shipId;
        }

        public void Deactivate()
        {
            Deactivated = true;
            DeactivateAtUtc = DateTime.UtcNow;
        }

        public void ValidateShip(Ship ship)
        {
            if (ship.Id != ShipId)
                throw new YagoException("Несовпадение идентификаторов Ship.Id и Colony.ShipId");
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
            FirstWedding = true;
        }
    }
}
