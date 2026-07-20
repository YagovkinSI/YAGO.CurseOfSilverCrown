using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Buildings
{
    public static class BuildingDataset
    {
        public static IReadOnlyList<IBuilding> Get()
        {
            return
            [
                new BuildingAdministrative(),
                new BuildingMining(),
                new BuildingService(),
                new BuildingProduction(),
            ];
        }

        public static IBuilding GetByType(IndustryType industryType)
        {
            return industryType switch
            {
                IndustryType.Administrative => new BuildingAdministrative(),
                IndustryType.Mining => new BuildingMining(),
                IndustryType.Service => new BuildingService(),
                IndustryType.Production => new BuildingProduction(),
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
