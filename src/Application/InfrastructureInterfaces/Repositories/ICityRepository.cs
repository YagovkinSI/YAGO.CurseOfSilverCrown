using System.Threading;
using System.Threading.Tasks;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface ICityRepository
    {
        Task<long> CreateNew(long userId, CancellationToken cancellationToken);
    }
}
