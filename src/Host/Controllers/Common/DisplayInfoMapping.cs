using YAGO.World.Domain.Common;

namespace YAGO.World.Host.Controllers.Common
{
    public static class DisplayInfoMapping
    {
        public static DisplayInfoResponse ToResponse(this DisplayInfo displayInfo)
        {
            return new DisplayInfoResponse(
                displayInfo.Name,
                displayInfo.ImageName,
                displayInfo.Description);
        }
    }
}
