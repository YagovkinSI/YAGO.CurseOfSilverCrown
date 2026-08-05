using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Mappings;
using YAGO.World.Host.Controllers.Colonies;

namespace YAGO.World.Host.Controllers.Buildings
{
    public static class ColonyBuildingMapping
    {
        public static MyBuilding ToMyBuilding(this ColonyIndustry colonyBuilding, ColonyState colonyState)
        {
            var type = GetType(colonyBuilding.Type);

            var buidingContext = colonyState.GetBuildingContext();
            var settings = colonyBuilding.GetBuilding(isPrivate: true, buidingContext);
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

        private static string GetType(ColonyIndustryType type)
        {
            return type switch
            {
                ColonyIndustryType.Administrative => BuildingResponseTypes.Administrative,
                ColonyIndustryType.Mining => BuildingResponseTypes.Mining,
                ColonyIndustryType.Service => BuildingResponseTypes.Service,
                ColonyIndustryType.Production => BuildingResponseTypes.Production,
                _ => throw new NotImplementedException(),
            };
        }

        private static MyBuildingBase GetMyBuildingPrivate(ColonyIndustry industry, ColonyState colonyState)
        {
            var buidingContext = colonyState.GetBuildingContext();
            var building = industry.GetBuilding(isPrivate: true, buidingContext);
            var (available, reason) = building.IsBuildAvailable(isPrivate: true, colonyState);
            var solarDelta = building.SolarsDelta;
            var bonuses = new Dictionary<StateKey, double[]>()
            {
                { StateKey.SolarsDelta, [solarDelta] }
            };
            return new MyBuildingBase(
                IsPrivate: true,
                industry.PrivateCount,
                available,
                reason,
                building.Investment / 5,
                bonuses.Select(x => x.MapToColonyPatameters()).ToList());
        }

        private static MyBuildingBase GetMyBuildingState(ColonyIndustry industry, ColonyState colonyState)
        {
            var buildingContext = colonyState.GetBuildingContext();
            var building = industry.GetBuilding(isPrivate: false, buildingContext);
            var (available, reason) = building.IsBuildAvailable(isPrivate: false, colonyState);
            var solarDelta = building.SolarsDelta;
            var bonuses = new Dictionary<StateKey, double[]>()
            {
                { StateKey.SolarsDelta, [solarDelta] }
            };
            return new MyBuildingBase(
                IsPrivate: false,
                industry.StateCount,
                available,
                reason,
                building.Investment,
                bonuses.Select(x => x.MapToColonyPatameters()).ToList());
        }

        public static ColonyIndustryType ToDomainType(string type)
        {
            return type switch
            {
                BuildingResponseTypes.Administrative => ColonyIndustryType.Administrative,
                BuildingResponseTypes.Mining => ColonyIndustryType.Mining,
                BuildingResponseTypes.Service => ColonyIndustryType.Service,
                BuildingResponseTypes.Production => ColonyIndustryType.Production,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
