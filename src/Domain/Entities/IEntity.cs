namespace YAGO.World.Domain.Common.Entities
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
    }
}
