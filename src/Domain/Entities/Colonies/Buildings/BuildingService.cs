using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Mappings;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class BuildingService : Building
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Service;

        public override string Name => "Модуль сферы услуг";
        public override string ImageName => ImageSet.ServiceCompany;
        public override string[] Description => ["Компания будет оказывать услуги растущему населению."];

        public override double Investment => 1000;

        public override double GdpTypeFactor => 1;
        public override double ModulesUsedTypeFactor => 1.2;
        public override double PopulationTypeFactor => 1;
        protected override double SolarsDeltaFactor => 1;

        public BuildingService(
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

            if (colonyState.GetServiceNeed() < 1)
                return (false, "Недостаточно населения для необходимого спроса.");

            var cost = isPrivate ? Investment / 5 : Investment;
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
