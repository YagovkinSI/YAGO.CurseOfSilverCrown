using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

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
        /// Солары
        /// </summary>
        public double Solars { get; private set; }

        /// <summary>
        /// Эффект от праздника
        /// </summary>
        public double FestivalEffect { get; private set; }

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
        /// Контракты колонии
        /// </summary>
        public IReadOnlyList<long> CompanyIds { get; private set; }

        /// <summary>
        /// Флаг деактивации колонии игроком
        /// </summary>
        public bool Deactivated { get; private set; }

        /// <summary>
        /// Время деактивации колонии игроком
        /// </summary>
        public DateTime? DeactivateAtUtc { get; private set; }

        /// <summary>
        /// Текущая неделя
        /// </summary>
        public int CurrentWeek { get; private set; }

        /// <summary>
        /// Пройденные эпизоды
        /// </summary>
        public Dictionary<long, string> Episodes { get; private set; }

        public Colony(
            long id,
            long userId,
            string name,
            double solars,
            double festivalEffect,
            bool firstWedding,
            long shipId,
            CodeOfLaws startGavernorType,
            IReadOnlyList<long> companyIds,
            bool deactivated,
            DateTime? deactivateAtUtc,
            int сurrentWeek,
            Dictionary<long, string> episodes)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            FestivalEffect = festivalEffect;
            FirstWedding = firstWedding;
            ShipId = shipId;
            CodeOfLaws = startGavernorType;
            CompanyIds = companyIds;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
            CurrentWeek = сurrentWeek;
            Episodes = episodes;
        }

        public static Colony CreateNew(
            long userId,
            string name,
            CodeOfLaws gavernorType)
        {
            return new Colony(
                id: default,
                userId: userId,
                name: name,
                solars: 1000,
                festivalEffect: 0,
                firstWedding: false,
                shipId: 1,
                startGavernorType: gavernorType,
                companyIds: [],
                deactivated: false,
                deactivateAtUtc: null,
                сurrentWeek: 0,
                episodes: new Dictionary<long, string>()
            );
        }

        public void AddSolars(double value)
        {
            Solars += value;
        }

        public void AddCompany(long companyId)
        {
            var companyIds = CompanyIds.ToList();
            companyIds.Add(companyId);
            CompanyIds = companyIds;
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
            if (companies.Companies.Count != CompanyIds.Count)
                throw new YagoException("Несовпадение количества Colony.Сontracts и Сontracts");

            if (!CompanyIds
                    .OrderBy(x => x)
                    .SequenceEqual(companies.Companies.Select(x => x.Id).OrderBy(x => x)))
            {
                throw new YagoException("Несовпадение Colony.Сontracts и Сontracts");
            }
        }

        public void AddFestivalEffect(double effect)
        {
            FestivalEffect += effect;
        }

        internal void AddWeek()
        {
            CurrentWeek++;
        }

        internal void SetFirstWedding()
        {
            FirstWedding = true;
        }
    }
}
