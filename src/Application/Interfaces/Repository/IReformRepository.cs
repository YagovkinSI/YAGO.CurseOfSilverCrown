using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Reforms;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IReformRepository
    {
        Task<Reform> Get(string code, CancellationToken cancellationToken);
        Task<IReadOnlyList<Reform>> GetAll(CancellationToken cancellationToken);
    }
}
