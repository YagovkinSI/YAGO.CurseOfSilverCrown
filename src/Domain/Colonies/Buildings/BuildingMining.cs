using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Colonies.Buildings
{
    public class BuildingMining : Building
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Mining;

        public override string Name => "Шахтёрская бригада";
        public override string ImageName => ImageSet.MiningBrigade;
        public override string[] Description => ["Добыча драгоценных и редкоземельных металлов с астероида - наиболее прибыльное дело в Поясе."];

        public override double Investment => 1500;

        public override double GdpTypeFactor => 1.3;
        public override double ModulesUsedTypeFactor => 0.8;
        public override double PopulationTypeFactor => 1;
        protected override double SolarsDeltaFactor => 1;

        public BuildingMining(
            bool isPrivate,
            BuildingContext context)
            : base(isPrivate, context)
        {
        }

        public override (bool isBuildAvailable, string? reason) IsBuildAvailable(bool isPrivate, ColonyState colonyState)
        {
            var slots = colonyState.Slots[Slots.ColonySlotType.Mining].GetFree(colonyState);
            if (slots < 1)
                return (false, "Достигнут лимит модулей на этом астероиде. Дальнейшее расширение невозможно на данном этапе.");

            var (isBuildAvailable, reason) = IsBuildAvailableBase(isPrivate, colonyState);
            if (!isBuildAvailable)
                return (isBuildAvailable, reason);

            return (true, null);
        }
    }
}
