using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Cities;

namespace YAGO.World.Application.Cities
{
    public interface ICityRepository
    {
        Task<City?> Find(long id, CancellationToken cancellationToken);

        Task<City?> FindByUser(long userId, CancellationToken cancellationToken);

        Task<City> Create(long userId, string name, string description, CancellationToken cancellationToken);

        Task<string[]> GetRandomCityNames(int count, CancellationToken cancellationToken);
    }
}
