namespace YAGO.World.Domain.Entities.Colonies.Resources
{
    public class ColonySolars : ColonyResource
    {
        public override ColonyResourceType Type => ColonyResourceType.Solars;
        public override double MinValue => double.MinValue;
        public override double MaxValue => double.MaxValue;

        public ColonySolars(double value) : base(value)
        {
        }

        public override double GetDeltaPerTurn(ColonyState colonyState)
        {
            var result = 0.0;

            foreach (var building in colonyState.Buildings.Values)
            {
                var privateBuildingCount = building.PrivateCount;
                var stateOwnedBuildingCount = building.StateCount;
                var buildingSettings = building.GetSettings();
                result += (privateBuildingCount + (3 * stateOwnedBuildingCount)) * buildingSettings.SolarsIncome;
            }
            return result;
        }
    }
}
