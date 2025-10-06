using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Cities;

namespace YAGO.World.Application.Cities
{
    public interface ICityService
    {
        Task<City?> GetMyCity(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);

        Task<string[]> GetRandomCityNames(CancellationToken cancellationToken);
    }
}
