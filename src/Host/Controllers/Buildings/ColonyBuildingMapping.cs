using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Buildings;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Host.Controllers.Buildings
{
    public static class ColonyBuildingMapping
    {
        public static MyBuilding ToMyBuilding(this ColonyBuilding colonyBuilding, ColonyState colonyState)
        {
            var type = GetType(colonyBuilding.Type);
            var name = GetName(colonyBuilding.Type);
            var imageName = GetImageName(colonyBuilding.Type);
            var description = GetDescription(colonyBuilding.Type);
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
                colonyBuilding.PrivateCount,
                available,
                reason,
                colonyBuilding.GetSettings().Cost);
        }

        private static string GetName(ColonyBuildingType type)
        {
            return type switch
            {
                ColonyBuildingType.Administrative => "Административный отдел",
                ColonyBuildingType.Mining => "Модуль добычи",
                ColonyBuildingType.Service => "Модуль сферы услуг",
                ColonyBuildingType.Production => "Модуль производства",
                _ => throw new NotImplementedException(),
            };
        }

        private static string GetImageName(ColonyBuildingType type)
        {
            return type switch
            {
                ColonyBuildingType.Administrative => ImageSet.Unknown,
                ColonyBuildingType.Mining => ImageSet.MiningBrigade,
                ColonyBuildingType.Service => ImageSet.ServiceCompany,
                ColonyBuildingType.Production => ImageSet.ProductionCompany,
                _ => throw new NotImplementedException(),
            };
        }

        private static string[] GetDescription(ColonyBuildingType type)
        {
            return type switch
            {
                ColonyBuildingType.Administrative => ["Управление персоналом, учёт ресурсов, отчётность перед Консорциумом. Здесь же работают советники правителя."],
                ColonyBuildingType.Mining => ["Добыча ресурсов на астероиде."],
                ColonyBuildingType.Service => ["Оказание услуг населению станции."],
                ColonyBuildingType.Production => ["Различного рода производство на станции."],
                _ => throw new NotImplementedException(),
            };
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
