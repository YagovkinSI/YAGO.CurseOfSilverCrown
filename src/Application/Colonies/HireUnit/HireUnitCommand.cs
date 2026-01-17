using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.HireUnit
{
    public class HireUnitCommand : IProcessorCommand
    {
        public long UserId { get; }
        public long UnitId { get; }

        public HireUnitCommand(long userId, long unitId)
        {
            UserId = userId;
            UnitId = unitId;
        }
    }
}
