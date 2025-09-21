using YAGO.World.Domain.Common.Entities.Enums;
using YAGO.World.Domain.Common.Entities.Interfaces;

namespace YAGO.World.Domain.Cities
{
    /// <summary>
    /// Полис игрока
    /// </summary>
    public class City : IEntity
    {
        public EntityType Type => EntityType.City;

        /// <summary>
        /// Идентификатор полиса
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Название полиса
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Идентификатор пользователя владельца
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Казна
        /// </summary>
        public int Gold { get; }

        /// <summary>
        /// Население
        /// </summary>
        public int Population { get; }

        /// <summary>
        /// Военная сила
        /// </summary>
        public int Military { get; }

        /// <summary>
        /// Уровень укреплений
        /// </summary>
        public int Fortifications { get; }

        /// <summary>
        /// Уровень контроля
        /// </summary>
        public int Control { get; }

        public City(
            long id,
            string name,
            long userId,
            int gold,
            int population,
            int military,
            int fortifications,
            int control)
        {
            Id = id;
            Name = name;
            UserId = userId;
            Gold = gold;
            Population = population;
            Military = military;
            Fortifications = fortifications;
            Control = control;
        }
    }
}
