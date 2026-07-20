using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    /// <summary>
    /// Отрасль колонии
    /// </summary>
    public interface IIndustry
    {
        /// <summary>
        /// Тип отрасли
        /// </summary>
        IndustryType Type { get; }

        /// <summary>
        /// Количество частных построек
        /// </summary>
        int PrivateBuildingCount { get; }

        /// <summary>
        /// Количество муниципальных построек
        /// </summary>
        int StateOwnedBuildingCount { get; }
    }
}
