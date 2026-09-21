using System;

namespace YAGO.World.Domain.Colonies.Buildings
{
    public static class BuildingContextMapping
    {
        public static BuildingContext GetBuildingContext(this ColonyState colonyState)
        {
            var corporateTaxRate = (float)colonyState.Reforms.CorporateTaxRate.Value;
            var stability = colonyState.GetStability();
            var logisticEffect = colonyState.Asteroid == null
                ? 1.0
                : 0.775 / Math.Sqrt(colonyState.Asteroid.DistanceToCeres);
            return new BuildingContext(corporateTaxRate, stability, logisticEffect);
        }
    }
}
