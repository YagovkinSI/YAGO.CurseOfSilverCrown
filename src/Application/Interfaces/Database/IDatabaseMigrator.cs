using System.Threading;
using System.Threading.Tasks;

namespace YAGO.World.Application.Interfaces.Database
{
    public interface IDatabaseMigrator
    {
        Task Initialize(CancellationToken cancellationToken);
    }
}
