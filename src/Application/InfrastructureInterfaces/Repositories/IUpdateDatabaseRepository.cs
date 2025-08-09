using System.Threading;
using System.Threading.Tasks;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IUpdateDatabaseRepository
    {
        public Task Update(CancellationToken cancellationToken);
    }
}
