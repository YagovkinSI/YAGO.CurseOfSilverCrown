using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Episodes;

namespace YAGO.World.Domain.Colonies.Buildings
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
            var (isBuildAvailable, reason) = IsBuildAvailableBase(isPrivate, colonyState);
            if (!isBuildAvailable)
                return (isBuildAvailable, reason);

            if (colonyState.GetServiceNeed() < 1)
                return (false, "Недостаточно населения для необходимого спроса.");

            return (true, null);
        }
    }
}
