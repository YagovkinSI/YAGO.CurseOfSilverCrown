using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Buildings;

namespace YAGO.World.Host.Controllers.Buildings
{
    public static class ColonyBuildingMapping
    {
        public static MyBuilding ToMyBuilding(this ColonyBuilding colonyBuilding, ColonyState colonyState)
        {
            var type = GetType(colonyBuilding.Type);
            var settings = colonyBuilding.GetSettings();
            var name = settings.Name;
            var imageName = settings.ImageName;
            var description = settings.Description;
            var myBuildingPrivate = GetMyBuildingPrivate(colonyBuilding, colonyState);
            var myBuildingState = GetMyBuildingState(colonyBuilding, colonyState);
            return new MyBuilding(
                type,
                name,
                imageName,
                description,
                myBuildingPrivate,
                myBuildingState);
        }

        private static string GetType(ColonyBuildingType type)
        {
            return type switch
            {
                ColonyBuildingType.Administrative => BuildingResponseTypes.Administrative,
                ColonyBuildingType.Mining => BuildingResponseTypes.Mining,
                ColonyBuildingType.Service => BuildingResponseTypes.Service,
                ColonyBuildingType.Production => BuildingResponseTypes.Production,
                _ => throw new NotImplementedException(),
            };
        }

        private static MyBuildingBase GetMyBuildingPrivate(ColonyBuilding colonyBuilding, ColonyState colonyState)
        {
            var (available, reason) = colonyBuilding.IsBuildAvailable(isPrivate: true, colonyState);
            return new MyBuildingBase(
                IsPrivate: true,
                colonyBuilding.PrivateCount,
                available,
                reason,
                Cost: 0);
        }

        private static MyBuildingBase GetMyBuildingState(ColonyBuilding colonyBuilding, ColonyState colonyState)
        {
            var (available, reason) = colonyBuilding.IsBuildAvailable(isPrivate: false, colonyState);
            return new MyBuildingBase(
                IsPrivate: false,
                colonyBuilding.StateCount,
                available,
                reason,
                colonyBuilding.GetSettings().Cost);
        }

        public static ColonyBuildingType ToDomainType(string type)
        {
            return type switch
            {
                BuildingResponseTypes.Administrative => ColonyBuildingType.Administrative,
                BuildingResponseTypes.Mining => ColonyBuildingType.Mining,
                BuildingResponseTypes.Service => ColonyBuildingType.Service,
                BuildingResponseTypes.Production => ColonyBuildingType.Production,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
