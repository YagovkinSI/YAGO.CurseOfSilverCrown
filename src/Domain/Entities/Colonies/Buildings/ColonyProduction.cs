using System;
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

            if (isPrivate)
            {
                if (colonyState.Reforms[ColonyReformType.TaxLevel].Value +
                    colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value > 6)
                    return (false, "Производство не рентабельно.");
                var competition = colonyState.GetAttractiveness() - Total;
                if (competition < 1)
                    return (false, $"Возможно через ходов: {(int)Math.Ceiling((1 - competition) * 3)}");
            }
            else
            {
                if (colonyState.Resources[Resources.ColonyResourceType.Solars].Value < settings.Cost)
                    return (false, "Недостаточно Солар.");
            }

            return (true, null);
        }
    }
}
