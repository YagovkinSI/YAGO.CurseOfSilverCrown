namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class ColonyService : ColonyBuilding
    {
        public override ColonyBuildingType Type => ColonyBuildingType.Service;

        public ColonyService(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        internal override BuildingSettings GetSettings()
        {
            return new BuildingSettings(
                Type,
                cost: 1000,
                zonesOccupied: 3,
                population: 10,
                solarsIncome: 12);
        }
    }
}
