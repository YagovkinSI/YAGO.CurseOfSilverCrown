using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Colonies.Buildings
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
            var (isBuildAvailable, reason) = IsBuildAvailableBase(isPrivate, colonyState);
            if (!isBuildAvailable)
                return (isBuildAvailable, reason);

            return (true, null);
        }
    }
}
