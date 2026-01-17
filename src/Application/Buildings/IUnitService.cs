using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Units;

namespace YAGO.World.Application.Buildings
{
    public interface IUnitService
    {
        Task<Unit?> GetUnit(long unitId, CancellationToken cancellationToken);
    }
}
