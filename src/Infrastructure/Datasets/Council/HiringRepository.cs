using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Infrastructure.Datasets.Council
{
    internal class HiringRepository : IHiringRepository
    {
        public Task<GameAction> Get(string personCode, CancellationToken cancellationToken)
        {
            var result = HiringDataset.Get(personCode);
            return Task.FromResult(result);
        }
    }
}