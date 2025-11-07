using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyWithShipAndBuildingsRepository
    {
        Task<ColonyWithShipAndBuildings?> Find(long colonyId, CancellationToken cancellationToken);
    }
}
