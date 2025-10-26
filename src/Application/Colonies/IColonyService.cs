using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyService
    {
        Task<Colony?> GetMyColony(long userId, CancellationToken cancellationToken);
        Task<ColonyWithShipAndBuildingsDto?> GetMyColonyWithShipAndBuildings(long userId, CancellationToken cancellationToken);
        Task<ColonyWithShipAndBuildingsDto> CreateColony(long userId, string name, ColonyPresetType presetType, CancellationToken cancellationToken);
        Task<ColonyWithShipAndBuildingsDto> BuyBuilding(long userId, long buildingId, CancellationToken cancellationToken);
    }
}
