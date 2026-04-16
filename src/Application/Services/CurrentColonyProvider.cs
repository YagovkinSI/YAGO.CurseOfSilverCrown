using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Services
{
    public interface ICurrentColonyProvider
    {
        Task<Colony> Get(long userId, CancellationToken cancellationToken);
    }

    public class CurrentColonyProvider(
        IColonyRepository colonyRepository)
        : ICurrentColonyProvider
    {
        public async Task<Colony> Get(long userId, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(userId, cancellationToken);
            if (colony == null)
            {
                colony = Colony.CreateNew(userId);
                await colonyRepository.Add(colony, cancellationToken);
            }

            return colony;
        }
    }
}
