using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Units;

namespace YAGO.World.Application.Buildings
{
    public class UnitService : IUnitService
    {
        public Task<Unit?> GetUnit(long unitId, CancellationToken cancellationToken)
        {
            var result = UnitsDataset.Get().FirstOrDefault(x => x.Id == unitId);
            return Task.FromResult(result);
        }
    }
}
