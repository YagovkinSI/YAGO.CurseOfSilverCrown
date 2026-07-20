namespace YAGO.World.Domain.Entities.Buildings
{
    /// <summary>
    /// Постройка
    /// </summary>
    public interface IBuilding
    {
        /// <summary>
        /// Тип пострйоки
        /// </summary>
        IndustryType Type { get; }

        /// <summary>
        /// Стоимость пострйоки
        /// </summary>
        double Cost { get; }

        /// <summary>
        /// Занимаеммая площадь
        /// </summary>
        int ZonesOccupied { get; }

        /// <summary>
        /// Занятое население (включая иждевенциев)
        /// </summary>
        int Population { get; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        double SolarsIncome { get; }
    }
}
