namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class IndustryEntity
    {
        public int CompanyCount { get; set; }
        public int ZonesOccupied { get; set; }
        public int SolarsIncome { get; set; }
        public int Population { get; set; }

        public IndustryEntity() { }

        public IndustryEntity(
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
    }
}
