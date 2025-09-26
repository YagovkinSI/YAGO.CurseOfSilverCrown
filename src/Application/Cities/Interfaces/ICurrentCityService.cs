using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Cities;

namespace YAGO.World.Application.Cities.Interfaces
{
    public interface ICurrentCityService
    {
        Task<City?> GetCurrentCity(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
    }
}
