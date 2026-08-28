using System;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Application.Common.Extensions;

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
            };
        }

        private static MyBuildingBase GetMyBuilding(ColonyIndustry industry, ColonyState colonyState, bool isPrivate)
        {
            var buidingContext = colonyState.GetBuildingContext();
            var building = industry.GetBuilding(isPrivate, buidingContext);
            var (available, reason) = building.IsBuildAvailable(isPrivate, colonyState);
            var solarDelta = building.SolarsDelta;
            var bonus = new ColonyParameterResponse(
                ColonyParameterNames.Economic_Budget_Balance,
                StatMenus: [], Weight: 0,
                "Солары за ход",
                solarDelta.ToBeautifulString(setPlus: true));
            return new MyBuildingBase(
                isPrivate,
                isPrivate ? industry.PrivateCount : industry.StateCount,
                available,
                reason,
                building.Cost,
                [bonus]);
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
