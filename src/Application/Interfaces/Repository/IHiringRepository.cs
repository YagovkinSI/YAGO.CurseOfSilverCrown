using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IHiringRepository
    {
        Task<GameAction> Get(string personCode, CancellationToken cancellationToken);
    }
}