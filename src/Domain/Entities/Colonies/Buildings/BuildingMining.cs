using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Mappings;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class BuildingMining : Building
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Mining;

        public override string Name => "Шахтёрская бригада";
        public override string ImageName => ImageSet.MiningBrigade;
        public override string[] Description => ["Добыча драгоценных и редкоземельных металлов с астероида - наиболее прибыльное дело в Поясе."];

        public override double Investment => 1000;

        public override double GdpTypeFactor => 1.3;
        public override double ModulesUsedTypeFactor => 0.8;
        public override double PopulationTypeFactor => 1;

        protected override double SolarsDeltaDefault => 90;


        public BuildingMining(
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

            var slots = colonyState.Slots[Slots.ColonySlotType.Mining].GetFree(colonyState);
            if (slots < 1)
                return (false, "Достигнут лимит модулей на этом астероиде. Дальнейшее расширение невозможно на данном этапе.");

            var cost = isPrivate ? Investment / 5 : Investment;
            if (colonyState.Resources[Resources.ColonyResourceType.Solars].Value < cost)
                return (false, "Недостаточно Солар.");

            if (isPrivate
                && colonyState.Reforms[ColonyReformType.TaxLevel].Value +
                    colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value > 6)
                return (false, "Добыча не рентабельна.");

            return (true, null);
        }
    }
}
