namespace YAGO.World.Domain.Entities.Buildings
{
    public class BuildingAdministrative : IBuilding
    {
        public IndustryType Type => IndustryType.Administrative;

        public double Cost => 1000;

        public int ZonesOccupied => 3;

        public int Population => 10;

        public double SolarsIncome => -10;
    }
}
