using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Colonies.Ships
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
        /// Стоимость взноса
        /// </summary>
        public int Contribution { get; }

        /// <summary>
        /// Стоимость содержания
        /// </summary>
        public int Maintenance { get; }

        /// <summary>
        /// Площадь под застройку
        /// </summary>
        public int Zones { get; }

        public Ship(
            long id,
            string name,
            string descriptionFooter,
            int contribution,
            int maintenance,
            int zones)
        {
            Id = id;
            Name = name;
            DescriptionFooter = descriptionFooter;
            Contribution = contribution;
            Maintenance = maintenance;
            Zones = zones;
        }
    }
}
