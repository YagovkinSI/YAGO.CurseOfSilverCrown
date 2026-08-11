using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Domain.Colonies.Buildings
{
    public class BuildingAdministrative : Building
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Administrative;

        public override string Name => "Администрация";
        public override string ImageName => ImageSet.Unknown;
        public override string[] Description => ["Управление персоналом, учёт ресурсов, отчётность перед Консорциумом."];

        public override double Investment => 2500;

        public override double GdpTypeFactor => 1;
        public override double ModulesUsedTypeFactor => 0.8;
        public override double PopulationTypeFactor => 1;
        protected override double SolarsDeltaFactor => 0;

        public BuildingAdministrative(
            bool isPrivate,
            BuildingContext context)
            : base(isPrivate, context)
        {
        }

        public override (bool isBuildAvailable, string? reason) IsBuildAvailable(
            bool isPrivate,
            ColonyState colonyState)
        {
            return (false, "В разработке");
        }
    }
}
