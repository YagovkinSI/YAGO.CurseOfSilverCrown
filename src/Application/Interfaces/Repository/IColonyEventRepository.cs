using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IColonyEventRepository
    {
        Task<ColonyEvent?> Find(long colonyEventId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ColonyEvent>> FindByColonyId(long colonyId, bool onlyNotComplited, CancellationToken cancellationToken);
        Task<ColonyEvent> Add(ColonyEvent colonyEvent, CancellationToken cancellationToken);
        Task Update(ColonyEvent colonyEvent, CancellationToken cancellationToken);
    }
}
