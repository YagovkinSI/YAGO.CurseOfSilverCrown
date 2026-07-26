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
            var name = GetName(colonyBuilding.Type);
            var imageName = GetImageName(colonyBuilding.Type);
            var description = GetDescription(colonyBuilding.Type);
            var myBuildingPrivate = GetMyBuildingPrivate(colonyBuilding, colonyState);
            var myBuildingState = GetMyBuildingState(colonyBuilding, colonyState);
            return new MyBuilding(
                name,
                imageName,
                description,
                myBuildingPrivate,
                myBuildingState);
        }

        private static MyBuildingPrivate GetMyBuildingPrivate(ColonyBuilding colonyBuilding, ColonyState colonyState)
        {
            var (available, reason) = colonyBuilding.IsBuildAvailable(isPrivate: true, colonyState);
            return new MyBuildingPrivate(
                colonyBuilding.PrivateCount,
                available,
                reason,
                colonyBuilding.GetSettings().Cost);
        }

        private static MyBuildingState GetMyBuildingState(ColonyBuilding colonyBuilding, ColonyState colonyState)
        {
            var (available, reason) = colonyBuilding.IsBuildAvailable(isPrivate: false, colonyState);
            return new MyBuildingState(
                colonyBuilding.PrivateCount,
                available,
                reason,
                cost: 0);
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
    }
}
