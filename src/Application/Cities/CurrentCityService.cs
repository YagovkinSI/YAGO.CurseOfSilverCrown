using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cities.Interfaces;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Cities;

namespace YAGO.World.Application.Cities
{
    public class CurrentCityService : ICurrentCityService
    {
        public readonly ICurrentUserService _currentUserService;
        public readonly ICityRepository _cityRepository;

        public CurrentCityService(ICurrentUserService currentUserService, ICityRepository cityRepository)
        {
            _currentUserService = currentUserService;
            _cityRepository = cityRepository;
        }

        public async Task<City?> GetCurrentCity(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetCurrentUser(userClaimsPrincipal, cancellationToken);

            return currentUser == null
                ? null
                : await _cityRepository.GetCityByUserId(currentUser.Id, cancellationToken);
        }
    }
}
