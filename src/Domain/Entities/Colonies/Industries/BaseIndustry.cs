namespace YAGO.World.Domain.Entities.Colonies.Industries
{    
    public abstract class BaseIndustry : IIndustry
    {
        public int UnitCount { get; protected set; }
        public abstract int ZonesOccupied { get; protected set; }
        public abstract int SolarsIncome { get; protected set; }
        public abstract int Population { get; protected set; }

        protected BaseIndustry(
            int companyCount)
        {
            UnitCount = companyCount;
        }
    }
}
