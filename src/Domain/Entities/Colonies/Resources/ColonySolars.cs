using YAGO.World.Domain.Entities.Buildings;

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

            foreach (var industryType in ColonyState.IndustryTypes)
            {
                var privateBuildingCount = colonyState.GetBuildCount(industryType, isPrivate: true);
                var stateOwnedBuildingCount = colonyState.GetBuildCount(industryType, isPrivate: false);
                var building = BuildingDataset.GetByType(industryType);
                result += (privateBuildingCount + (3 * stateOwnedBuildingCount)) * building.SolarsIncome;
            }
            return result;
        }
    }
}
