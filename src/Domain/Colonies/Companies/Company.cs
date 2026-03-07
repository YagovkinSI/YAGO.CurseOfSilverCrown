namespace YAGO.World.Domain.Colonies.Companies
{
    /// <summary>
    /// ОТряд или юнит
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Площадь
        /// </summary>
        public int ZonesOccupied { get; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public int SolarsIncome { get; }

        /// <summary>
        /// Население
        /// </summary>
        public int Population { get; }

        public Company(
            long id,
            int zonesOccupied,
            int solarsIncome,
            int population)
        {
            Id = id;
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            Population = population;
        }
    }
}
