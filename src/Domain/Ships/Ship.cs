using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Ships
{
    /// <summary>
    /// Корабль
    /// </summary>
    public class Ship : IEntity
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
        /// Дополнительное описание
        /// </summary>
        public string DescriptionFooter { get; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Cost { get; }

        /// <summary>
        /// Площадь под застройку
        /// </summary>
        public int Zones { get; }

        /// <summary>
        /// Потребление соларов
        /// </summary>
        public decimal SolarsConsumption { get; }

        public Ship(
            long id,
            string name,
            string descriptionFooter,
            decimal cost,
            int zones,
            decimal solarsConsumption)
        {
            Id = id;
            Name = name;
            DescriptionFooter = descriptionFooter;
            Cost = cost;
            Zones = zones;
            SolarsConsumption = solarsConsumption;
        }

        public static Ship GetDefaultShip()
        {
            return new Ship(
                id: 1,
                "Рассвет-782",
                "Стандартный корабль-город для начинающих правителей. Скромный, но функциональный.",
                cost: 6500,
                zones: 140,
                solarsConsumption: 150);
        }
    }
}
