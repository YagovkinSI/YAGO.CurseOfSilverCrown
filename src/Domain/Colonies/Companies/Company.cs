using System;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Colonies.Companies
{
    /// <summary>
    /// ОТряд или юнит
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public int Cost { get; }

        /// <summary>
        /// Площадь
        /// </summary>
        public int ZonesOccupied { get; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public int SolarsIncome { get; }

        /// <summary>
        /// Репутация
        /// </summary>
        public CodeOfLaws GavernorType { get; }

        /// <summary>
        /// Население
        /// </summary>
        public int Population { get; }

        /// <summary>
        /// Текст
        /// </summary>
        public string[] Text { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public string[] Description { get; }

        public Company(
            long id,
            string name,
            int cost,
            int zonesOccupied,
            int solarsIncome,
            CodeOfLaws gavernorType,
            int population,
            string[] text,
            string[] description)
        {
            Id = id;
            Name = name;
            Cost = cost;
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            GavernorType = gavernorType;
            Population = population;
            Text = text;
            Description = description;
        }

        public void СoncludeСontract(Colony colony,
            Ship ship,
            ColonyCompanies companies)
        {
            colony.ValidateShip(ship);
            colony.ValidateContracts(companies);

            if (Math.Abs((int)GavernorType - (int)colony.CodeOfLaws) > 1)
                throw new YagoException("Недопустимый контракт для выбранных законов.");

            if (colony.Solars < Cost)
                throw new YagoException("Недостаточно средств.");

            var areaCapacity = new AreaCapacity(colony, companies, ship);
            if (ship.Zones - areaCapacity.Occupied < ZonesOccupied)
                throw new YagoException("Недостаточно секторов.");

            colony.AddSolars(-Cost);
            colony.AddCompany(Id);
            companies.AddCompany(this);
        }
    }
}
