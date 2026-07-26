namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class ColonyProduction : ColonyBuilding
    {
        public override ColonyBuildingType Type => ColonyBuildingType.Production;

        public ColonyProduction(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        internal override BuildingSettings GetSettings()
        {
            return new BuildingSettings(
                Type,
                cost: 2500,
                zonesOccupied: 5,
                population: 25,
                solarsIncome: 35);
        }
    }
}
