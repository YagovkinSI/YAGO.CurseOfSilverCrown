using YAGO.World.Domain.Common.Entities.Enums;

namespace YAGO.World.Domain.Common.Entities.Interfaces
{
    /// <summary>
    /// Общая сущность проекта
    /// </summary>
    public interface IEntity
    {        
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Тип сущности
        /// </summary>
        public EntityType Type { get; }

        /// <summary>
        /// Название сущности
        /// </summary>
        public string Name { get; }
    }
}
