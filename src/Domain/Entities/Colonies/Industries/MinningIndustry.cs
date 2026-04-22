namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class MinningIndustry : BaseIndustry
    {
        private const int MaxUnitCount = 12;

        public override int ZonesOccupied { get; protected set; }
        public override int SolarsIncome { get; protected set; }
        public override int Population { get; protected set; }
        public int UnitAvailable => MaxUnitCount - UnitCount;

        public MinningIndustry(
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

        public static MinningIndustry CreateNew()
        {
            return new MinningIndustry(
                companyCount: 0,
                zonesOccupied: 0,
                solarsIncome: 0,
                population: 0);
        }

        internal void AddCompany(int count, int zonesOccupied, int solarIncome, int population)
        {
            UnitCount += count;
            ZonesOccupied += zonesOccupied;
            SolarsIncome += solarIncome;
            Population += population;
        }
    }
}
