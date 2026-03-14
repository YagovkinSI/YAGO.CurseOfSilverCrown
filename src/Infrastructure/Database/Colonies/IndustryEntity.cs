namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class IndustryEntity
    {
        public string Name { get; set; }
        public int CompanyCount { get; set; }
        public int ZonesOccupied { get; set; }
        public int SolarsIncome { get; set; }
        public int Population { get; set; }

        public IndustryEntity() { }

        public IndustryEntity(
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
    }
}
