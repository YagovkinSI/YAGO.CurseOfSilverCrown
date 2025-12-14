using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.BuyBuilding
{
    public class BuyBuildingCommand : IProcessorCommand
    {
        public long UserId { get; }
        public long BuildingId { get; }

        public BuyBuildingCommand(long userId, long buildingId)
        {
            UserId = userId;
            BuildingId = buildingId;
        }
    }
}
