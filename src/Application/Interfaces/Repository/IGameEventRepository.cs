using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IGameEventRepository
    {
        Task<GameEvent> Get(string code, CancellationToken cancellationToken);
        Task<IReadOnlyList<GameEvent>> GetAll(CancellationToken cancellationToken);
    }
}
