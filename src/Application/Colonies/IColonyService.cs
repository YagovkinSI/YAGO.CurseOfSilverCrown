using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyService
    {
        Task<Colony?> GetMyColony(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<ColonyWithShipAndBuildingsDto?> GetMyColonyWithShipAndBuildings(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<ColonyWithShipAndBuildingsDto> CreateColony(ClaimsPrincipal userClaimsPrincipal, string name, ColonyPresetType presetType, CancellationToken cancellationToken);
        Task<ColonyWithShipAndBuildingsDto> BuyBuilding(ClaimsPrincipal userClaimsPrincipal, long buildingId, CancellationToken cancellationToken);
    }
}
