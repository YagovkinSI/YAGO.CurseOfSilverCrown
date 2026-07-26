namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class ColonyAdministrative : ColonyBuilding
    {
        public override ColonyBuildingType Type => ColonyBuildingType.Administrative;

        public ColonyAdministrative(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        internal override BuildingSettings GetSettings()
        {
            return new BuildingSettings(
                Type,
                cost: 1000,
                zonesOccupied: 3,
                population: 10,
                solarsIncome: -10);
        }
    }
}
