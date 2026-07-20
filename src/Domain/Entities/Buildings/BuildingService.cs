namespace YAGO.World.Domain.Entities.Buildings
{
    public class BuildingService : IBuilding
    {
        public IndustryType Type => IndustryType.Service;

        public double Cost => 1000;

        public int ZonesOccupied => 3;

        public int Population => 10;

        public double SolarsIncome => 12;
    }
}
