using System.Threading;
using System.Threading.Tasks;

namespace YAGO.World.Application.Common.Database
{
    public interface IDatabaseInitializer
    {
        Task Initialize(CancellationToken cancellationToken);
    }
}
