namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class IndustryEntity
    {
        public int PrivateBuildingCount { get; set; }
        public int StateOwnedBuildingCount { get; set; }

        public IndustryEntity() { }

        public IndustryEntity(
            int privateBuildingCount,
            int stateOwnedBuildingCount)
        {
            PrivateBuildingCount = privateBuildingCount;
            StateOwnedBuildingCount = stateOwnedBuildingCount;
        }
    }
}
