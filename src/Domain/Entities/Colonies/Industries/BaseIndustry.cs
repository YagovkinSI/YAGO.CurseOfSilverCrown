namespace YAGO.World.Domain.Entities.Colonies.Industries
{    
    public abstract class BaseIndustry : IIndustry
    {
        /// <summary>
        /// Количество компаний
        /// </summary>
        public int CompanyCount { get; private set; }

        public int ZonesOccupied { get; private set; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public int SolarsIncome { get; private set; }

        /// <summary>
        /// Население
        /// </summary>
        public int Population { get; private set; }

        protected BaseIndustry(
            int companyCount,
            int zonesOccupied,
            int solarsIncome,
            int population)
        {
            CompanyCount = companyCount;
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            Population = population;
        }

        internal void AddCompany(int count, int zonesOccupied, int solarIncome, int population)
        {
            CompanyCount += count;
            ZonesOccupied += zonesOccupied;
            SolarsIncome += solarIncome;
            Population += population;
        }
    }
}
