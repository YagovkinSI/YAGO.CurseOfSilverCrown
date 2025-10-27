using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record BuyBuildingRequest(
        [IdValidationAttribute]  long BuildingId);
}
