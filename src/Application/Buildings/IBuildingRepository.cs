using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Buildings;

namespace YAGO.World.Application.Buildings
{
    public interface IBuildingRepository
    {
        Task<Building?> Find(long buildingId, CancellationToken cancellationToken);
        Task<Building[]> GetBuildings(int page, int count, CancellationToken cancellationToken);
        Task<Building[]> GetBuildings(long[] buildingIds, CancellationToken cancellationToken);
    }
}
