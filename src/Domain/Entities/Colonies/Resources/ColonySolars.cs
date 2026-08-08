using YAGO.World.Domain.Mappings;
using YAGO.World.Domain.Reforms;

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

            var buildingContext = colonyState.GetBuildingContext();
            foreach (var industry in colonyState.Industries.Values)
            {
                var buildingPrivate = industry.GetBuilding(isPrivate: true, buildingContext);
                var privateBuildingCount = industry.PrivateCount;
                var solarDeltaPrivate = buildingPrivate.SolarsDelta;

                var buildingState = industry.GetBuilding(isPrivate: false, buildingContext);
                var stateOwnedBuildingCount = industry.StateCount;
                var solarDeltaState = buildingState.SolarsDelta;

                result += (privateBuildingCount * solarDeltaPrivate) + (stateOwnedBuildingCount * solarDeltaState);
            }

            var yagoLevel = colonyState.GetYagoLevel();
            var publicDeptContext = new PublicDebtContext(yagoLevel);
            var publicDept = new PublicDebt(colonyState.Reforms[ColonyReformType.PublicDebt].Value, publicDeptContext);

            return result + publicDept.SolarDelta;
        }
    }
}
