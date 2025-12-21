using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Buildings
{
    /// <summary>
    /// Постройка
    /// </summary>
    public class Building : IEntity
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
        public decimal Cost { get; }

        /// <summary>
        /// Площадь
        /// </summary>
        public int ZonesOccupied { get; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public decimal SolarsIncome { get; }

        /// <summary>
        /// Репутация
        /// </summary>
        public decimal Stability { get; }

        /// <summary>
        /// Население
        /// </summary>
        public int Population { get; }

        /// <summary>
        /// Население
        /// </summary>
        public string[] Description { get; }

        public Building(
            long id,
            string name,
            decimal cost,
            int zonesOccupied,
            decimal solarsIncome,
            decimal stability,
            int population,
            string[] description)
        {
            Id = id;
            Name = name;
            Cost = cost;
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            Stability = stability;
            Population = population;
            Description = description;
        }
    }
}
