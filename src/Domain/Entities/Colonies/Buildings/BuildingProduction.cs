using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Mappings;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class BuildingProduction : Building
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Production;

        public override string Name => "Модуль производства";
        public override string ImageName => ImageSet.ProductionCompany;
        public override string[] Description => ["Новые колонисты будут производить продукцию компании на нашей станции."];

        public override double Investment => 2500;

        public override double GdpTypeFactor => 1;
        public override double ModulesUsedTypeFactor => 1;
        public override double PopulationTypeFactor => 1;
        protected override double SolarsDeltaFactor => 1;

        public BuildingProduction(
            bool isPrivate,
            BuildingContext context)
            : base(isPrivate, context)
        {
        }

        public override (bool isBuildAvailable, string? reason) IsBuildAvailable(bool isPrivate, ColonyState colonyState)
        {
            var buildingContext = colonyState.GetBuildingContext();
            if (colonyState.Slots[Slots.ColonySlotType.Modules].GetFree(colonyState) < ModulesUsed)
                return (false, "Недостаточно модулей на станции.");

            var cost = isPrivate ? Investment / 5 : Investment;
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
