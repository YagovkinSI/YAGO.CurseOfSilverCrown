namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class AdministrativeIndustry : BaseIndustry
    {
        public override int ZonesOccupied { get; protected set; }
        public override int SolarsIncome { get; protected set; }
        public override int Population { get; protected set; }

        public AdministrativeIndustry(
            int companyCount,
            int zonesOccupied,
            int solarsIncome,
            int population)
            : base(companyCount)
        {
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            Population = population;
        }

        public static AdministrativeIndustry CreateNew()
        {
            return new AdministrativeIndustry(
                companyCount: 1,
                zonesOccupied: 0,
                solarsIncome: -60,
                population: 0);
        }
    }
}
