namespace YAGO.World.Domain.Common
{
    /// <summary>
    /// Общая сущность проекта
    /// </summary>
    public interface IEntity { }

    /// <summary>
    /// Общая сущность проекта
    /// </summary>
    public interface IEntity<out T> : IEntity
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public T Id { get; }
    }
}
