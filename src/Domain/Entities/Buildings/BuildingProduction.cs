namespace YAGO.World.Domain.Entities.Buildings
{
    public class BuildingProduction : IBuilding
    {
        public IndustryType Type => IndustryType.Production;

        public double Cost => 2500;

        public int ZonesOccupied => 5;

        public int Population => 25;

        public double SolarsIncome => 35;
    }
}
