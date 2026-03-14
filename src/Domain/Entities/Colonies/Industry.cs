namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Отрасль колонии
    /// </summary>
    public class Industry
    {
        /// <summary>
        /// Название отрасли
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Количество компаний
        /// </summary>
        public int CompanyCount { get; private set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public int ZonesOccupied { get; private set; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public int SolarsIncome { get; private set; }

        /// <summary>
        /// Население
        /// </summary>
        public int Population { get; private set; }

        public Industry(
            string name,
            int companyCount,
            int zonesOccupied,
            int solarsIncome,
            int population)
        {
            Name = name;
            CompanyCount = companyCount;
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            Population = population;
        }

        public static Industry CreateNewMinning()
        {
            return new Industry(
                IndustryNameConstants.Minning,
                companyCount: 4,
                zonesOccupied: 12,
                solarsIncome: 120,
                population: 60);
        }

        public static Industry CreateNewProduction()
        {
            return new Industry(
                IndustryNameConstants.Production,
                companyCount: 0,
                zonesOccupied: 0,
                solarsIncome: 0,
                population: 0);
        }

        public static Industry CreateNewService()
        {
            return new Industry(
                IndustryNameConstants.Service,
                companyCount: 0,
                zonesOccupied: 0,
                solarsIncome: 0,
                population: 0);
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
