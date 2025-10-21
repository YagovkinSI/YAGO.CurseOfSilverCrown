using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Buildings;

namespace YAGO.World.Application.Buildings
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;

        public BuildingService(
            IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        public async Task<Building?> GetBuilding(long buildingId, CancellationToken cancellationToken)
        {
            return await _buildingRepository.Find(buildingId, cancellationToken);
        }

        public async Task<Building[]> GetBuildings(int page, int count, CancellationToken cancellationToken)
        {
            return await _buildingRepository.GetBuildings(page, count, cancellationToken);
        }
    }
}
