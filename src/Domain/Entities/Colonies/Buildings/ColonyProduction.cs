using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class ColonyProduction : ColonyBuilding
    {
        public override ColonyBuildingType Type => ColonyBuildingType.Production;

        public ColonyProduction(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        public override BuildingSettings GetSettings()
        {
            return new BuildingSettings(
                Type,
                "Модуль производства",
                ImageSet.ProductionCompany,
                ["Новые колонисты будут производить продукцию компании на нашей станции."],
                cost: 2500,
                zonesOccupied: 5,
                population: 25,
                solarsIncome: 35);
        }

        public override (bool isBuildAvailable, string? reason) IsBuildAvailable(bool isPrivate, ColonyState colonyState)
        {
            var settings = GetSettings();
            if (colonyState.Slots[Slots.ColonySlotType.Modules].GetFree(colonyState) < settings.ZonesOccupied)
                return (false, "Недостаточно модулей на станции.");

            var cost = isPrivate ? settings.Cost / 5 : settings.Cost;
            if (colonyState.Resources[Resources.ColonyResourceType.Solars].Value < cost)
                return (false, "Недостаточно Солар.");

            if (isPrivate
                && colonyState.Reforms[ColonyReformType.TaxLevel].Value +
                    colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value > 6)
                return (false, "Производство не рентабельно.");

            return (true, null);
        }
    }
}
