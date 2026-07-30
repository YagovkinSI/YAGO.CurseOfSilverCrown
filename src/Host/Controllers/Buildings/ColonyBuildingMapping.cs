using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Buildings;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;

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
            var bonuses = new Dictionary<StateKey, double[]>()
            {
                { StateKey.SolarsDelta, [colonyBuilding.GetSettings().SolarsIncome] }
            };
            return new MyBuildingBase(
                IsPrivate: true,
                colonyBuilding.PrivateCount,
                available,
                reason,
                colonyBuilding.GetSettings().Cost / 5,
                bonuses.Select(x => x.MapToColonyPatameters()).ToList());
        }

        private static MyBuildingBase GetMyBuildingState(ColonyBuilding colonyBuilding, ColonyState colonyState)
        {
            var (available, reason) = colonyBuilding.IsBuildAvailable(isPrivate: false, colonyState);
            var bonuses = new Dictionary<StateKey, double[]>()
            {
                { StateKey.SolarsDelta, [3 * colonyBuilding.GetSettings().SolarsIncome] }
            };
            return new MyBuildingBase(
                IsPrivate: false,
                colonyBuilding.StateCount,
                available,
                reason,
                colonyBuilding.GetSettings().Cost,
                bonuses.Select(x => x.MapToColonyPatameters()).ToList());
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
