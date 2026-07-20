namespace YAGO.World.Domain.Entities.Buildings
{
    public class BuildingMining : IBuilding
    {
        public IndustryType Type => IndustryType.Mining;

        public double Cost => 1000;

        public int ZonesOccupied => 2;

        public int Population => 10;

        public double SolarsIncome => 30;
    }
}
