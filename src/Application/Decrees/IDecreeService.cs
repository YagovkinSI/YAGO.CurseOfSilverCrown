using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Decrees;

namespace YAGO.World.Application.Decrees
{
    public interface IDecreeService
    {
        Task<Decree?> GetDecree(long decreeId, CancellationToken cancellationToken);
    }
}
