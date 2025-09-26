using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Cities;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface ICityRepository
    {
        Task<long> CreateNew(long userId, CancellationToken cancellationToken);
        Task<City?> GetCityByUserId(long userId, CancellationToken cancellationToken);
    }
}
