namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class ColonyMining : ColonyBuilding
    {
        public override ColonyBuildingType Type => ColonyBuildingType.Mining;

        public ColonyMining(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        internal override BuildingSettings GetSettings()
        {
            return new BuildingSettings(
                Type,
                cost: 1000,
                zonesOccupied: 2,
                population: 10,
                solarsIncome: 30);
        }
    }
}
