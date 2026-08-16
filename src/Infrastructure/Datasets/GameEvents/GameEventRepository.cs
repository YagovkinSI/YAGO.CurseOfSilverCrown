using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    internal class GameEventRepository : IGameEventRepository
    {
        public Task<GameEvent> Get(string code, CancellationToken cancellationToken)
        {
            var result = GameEventsDataset.Get(code);
            return Task.FromResult(result); 
        }

        public Task<IReadOnlyList<GameEvent>> GetAll(CancellationToken cancellationToken)
        {
            var result = GameEventsDataset.All;
            return Task.FromResult(result);
        }
    }
}
