using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class ColonyAdministrative : ColonyBuilding
    {
        public override ColonyBuildingType Type => ColonyBuildingType.Administrative;

        public ColonyAdministrative(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        public override BuildingSettings GetSettings()
        {
            return new BuildingSettings(
                Type,
                "Администрация",
                ImageSet.Unknown,
                ["Управление персоналом, учёт ресурсов, отчётность перед Консорциумом."],
                cost: 1000,
                zonesOccupied: 3,
                population: 10,
                solarsIncome: -10);
        }

        public override (bool isBuildAvailable, string? reason) IsBuildAvailable(
            bool isPrivate,
            ColonyState colonyState)
        {
            return (false, "В разработке");
        }
    }
}
