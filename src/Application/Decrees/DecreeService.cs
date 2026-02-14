using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Decrees;

namespace YAGO.World.Application.Decrees
{
    public class DecreeService : IDecreeService
    {
        public Task<Decree?> GetDecree(long decreeId, CancellationToken cancellationToken)
        {
            var result = DecreeDataset.Get().FirstOrDefault(x => x.Id == decreeId);
            return Task.FromResult(result);
        }
    }
}
