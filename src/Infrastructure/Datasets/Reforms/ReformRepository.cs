using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Reforms;

namespace YAGO.World.Infrastructure.Datasets.Reforms
{
    internal class ReformRepository : IReformRepository
    {
        public Task<Reform> Get(string code, CancellationToken cancellationToken)
        {
            var result = ReformDataset.Get(code);
            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<Reform>> GetAll(CancellationToken cancellationToken)
        {
            var result = ReformDataset.All;
            return Task.FromResult(result);
        }
    }
}
