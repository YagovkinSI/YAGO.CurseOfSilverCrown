namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class MinningIndustry : BaseIndustry
    {
        public MinningIndustry(
            int companyCount,
            int zonesOccupied,
            int solarsIncome,
            int population)
            : base(companyCount, zonesOccupied, solarsIncome, population)
        {
        }

        public static MinningIndustry CreateNew()
        {
            return new MinningIndustry(
                companyCount: 4,
                zonesOccupied: 12,
                solarsIncome: 120,
                population: 60);
        }
    }
}
