using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.GameActions;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;

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
            var myBuildingPrivate = GetMyBuilding(colonyBuilding, colonyState, isPrivate: true);
            var myBuildingState = GetMyBuilding(colonyBuilding, colonyState, isPrivate: false);
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

        private static MyBuildingBase GetMyBuilding(ColonyIndustry industry, ColonyState colonyState, bool isPrivate)
        {
            var buidingContext = colonyState.GetBuildingContext();
            var building = industry.GetBuilding(isPrivate, buidingContext);
            var (available, reason) = building.IsBuildAvailable(isPrivate, colonyState);
            var solarDelta = building.SolarsDelta;
            var bonuses = new Dictionary<GameParameterType, double[]>()
            {
                { GameParameterType.SolarsDelta, [solarDelta] }
            };
            return new MyBuildingBase(
                isPrivate,
                isPrivate ? industry.PrivateCount : industry.StateCount,
                available,
                reason,
                building.Cost,
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
