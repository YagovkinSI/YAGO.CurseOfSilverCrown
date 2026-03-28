namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class ProductionIndustry : BaseIndustry
    {
        public ProductionIndustry(
            int companyCount,
            int zonesOccupied,
            int solarsIncome,
            int population)
            : base(companyCount, zonesOccupied, solarsIncome, population)
        {
        }

        public static ProductionIndustry CreateNew()
        {
            return new ProductionIndustry(
                companyCount: 0,
                zonesOccupied: 0,
                solarsIncome: 0,
                population: 0);
        }
    }
}
