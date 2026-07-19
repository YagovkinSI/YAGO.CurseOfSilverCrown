using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    /// <summary>
    /// Отрасль колонии
    /// </summary>
    public interface IIndustry
    {
        /// <summary>
        /// Количество частных построек
        /// </summary>
        public int PrivateBuildingCount { get; }

        /// <summary>
        /// Количество муниципальных построек
        /// </summary>
        public int StateOwnedBuildingCount { get; }

        /// <summary>
        /// Пострйока отрасли
        /// </summary>
        Building Building { get; }
    }
}
