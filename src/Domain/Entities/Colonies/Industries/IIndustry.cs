namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    /// <summary>
    /// Отрасль колонии
    /// </summary>
    public interface IIndustry
    {
        /// <summary>
        /// Количество подразделений
        /// </summary>
        public int UnitCount { get; }

        /// <summary>
        /// Занимаеммая площадь
        /// </summary>
        int ZonesOccupied { get; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public int SolarsIncome { get; }

        /// <summary>
        /// Занятое население (включая иждевенциев)
        /// </summary>
        public int Population { get; }
    }
}
