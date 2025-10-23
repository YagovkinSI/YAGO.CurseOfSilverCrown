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
        /// Солары
        /// </summary>
        public decimal Solars { get; }

        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId => 1;

        /// <summary>
        /// Идентифиикаторы построек
        /// </summary>
        public long[] BuildingIds { get; }

        public Colony(
            long id,
            long userId,
            string name,
            decimal solars,
            long[] buildingIds)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            BuildingIds = buildingIds;
        }
    }
}
