using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Turns;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface ITurnRepository
    {
        Task<Turn> Add(Turn turn, CancellationToken cancellationToken);
        Task<Turn?> Find(Guid turnId, CancellationToken cancellationToken);
        Task<Turn?> FindLastColonyTurn(Guid colonyId, CancellationToken cancellationToken);
    }
}
