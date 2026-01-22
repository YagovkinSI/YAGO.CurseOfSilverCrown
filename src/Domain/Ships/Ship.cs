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
        public int Cost { get; }

        /// <summary>
        /// Площадь под застройку
        /// </summary>
        public int Zones { get; }

        public Ship(
            long id,
            string name,
            string descriptionFooter,
            int cost,
            int zones)
        {
            Id = id;
            Name = name;
            DescriptionFooter = descriptionFooter;
            Cost = cost;
            Zones = zones;
        }

        public static Ship GetDefaultShip()
        {
            return new Ship(
                id: 1,
                "Рассвет-782",
                "Стандартный корабль-город для начинающих правителей. Скромный, но функциональный.",
                cost: 10000,
                zones: 140);
        }
    }
}
