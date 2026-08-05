using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Buildings;

namespace YAGO.World.Domain.Mappings
{
    public static class BuildingContextMapping
    {
        public static BuildingContext GetBuildingContext(this ColonyState colonyState)
        {
            var corporateTaxRate = colonyState.Reforms[ColonyReformType.TaxLevel].Value switch
            {
                1 => 5f,
                2 => 13f,
                3 => 20f,
                4 => 27f,
                5 => 35f,
                _ => throw new NotImplementedException()
            };
            return new BuildingContext(corporateTaxRate);
        }
    }
}
