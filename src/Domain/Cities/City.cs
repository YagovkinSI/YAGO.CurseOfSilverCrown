using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Cities
{
    /// <summary>
    /// Город игрока
    /// </summary>
    public class City : IEntity
    {
        /// <summary>
        /// Идентификатор города
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентификатор игрока владельца
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Название города
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Описание города
        /// </summary>
        public string Descripion { get; }

        public City(
            long id,
            long userId,
            string name,
            string descripion)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Descripion = descripion;
        }
    }
}
