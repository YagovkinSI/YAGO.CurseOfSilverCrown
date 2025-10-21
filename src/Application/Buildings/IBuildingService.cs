using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Buildings;

namespace YAGO.World.Application.Buildings
{
    public interface IBuildingService
    {
        Task<Building?> GetBuilding(long buildingId, CancellationToken cancellationToken);
    }
}
