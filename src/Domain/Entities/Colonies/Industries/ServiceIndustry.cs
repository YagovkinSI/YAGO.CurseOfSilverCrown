namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class ServiceIndustry : BaseIndustry
    {
        public ServiceIndustry(
            int companyCount,
            int zonesOccupied,
            int solarsIncome,
            int population)
            : base(companyCount, zonesOccupied, solarsIncome, population)
        {
        }

        public static ServiceIndustry CreateNew()
        {
            return new ServiceIndustry(
                companyCount: 0,
                zonesOccupied: 0,
                solarsIncome: 0,
                population: 0);
        }
    }
}
