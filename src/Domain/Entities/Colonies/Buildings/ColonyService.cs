using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class ColonyService : ColonyBuilding
    {
        public override ColonyBuildingType Type => ColonyBuildingType.Service;

        public ColonyService(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        public override BuildingSettings GetSettings()
        {
            return new BuildingSettings(
                Type,
                "Модуль сферы услуг",
                ImageSet.ServiceCompany,
                ["Компания будет оказывать услуги растущему населению."],
                cost: 1000,
                zonesOccupied: 3,
                population: 10,
                solarsIncome: 12);
        }

        public override (bool isBuildAvailable, string? reason) IsBuildAvailable(bool isPrivate, ColonyState colonyState)
        {
            var settings = GetSettings();
            if (colonyState.Slots[Slots.ColonySlotType.Modules].GetFree(colonyState) < settings.ZonesOccupied)
                return (false, "Недостаточно модулей на станции.");

            if (colonyState.GetServiceNeed() < 1)
                return (false, "Недостаточно населения для необходимого спроса.");

            var cost = isPrivate ? settings.Cost / 5 : settings.Cost;
            if (colonyState.Resources[Resources.ColonyResourceType.Solars].Value < cost)
                return (false, "Недостаточно Солар.");

            if (isPrivate
                && colonyState.Reforms[ColonyReformType.TaxLevel].Value +
                    colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value > 6)
                return (false, "Оказание услуг не рентабельно.");

            return (true, null);
        }
    }
}
