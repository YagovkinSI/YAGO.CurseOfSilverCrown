using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Decrees;

namespace YAGO.World.Application.Decrees
{
    public interface IDecreeService
    {
        Task<Decree?> GetDecree(long decreeId, CancellationToken cancellationToken);
    }
}
