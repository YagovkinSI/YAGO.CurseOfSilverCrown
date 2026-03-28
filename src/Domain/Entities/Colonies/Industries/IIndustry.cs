namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    /// <summary>
    /// Отрасль колонии
    /// </summary>
    internal interface IIndustry
    {
        /// <summary>
        /// Занимаеммая площадь
        /// </summary>
        int ZonesOccupied { get; }
    }
}
