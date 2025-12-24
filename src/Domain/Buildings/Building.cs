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
        public int Challenges { get; }

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
            int cost,
            int zonesOccupied,
            int solarsIncome,
            int challenges,
            int population,
            string[] description)
        {
            Id = id;
            Name = name;
            Cost = cost;
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            Challenges = challenges;
            Population = population;
            Description = description;
        }
    }
}
