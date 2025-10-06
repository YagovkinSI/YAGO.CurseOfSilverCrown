using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cities;
using YAGO.World.Domain.Cities;

namespace YAGO.World.Application.Users
{
    public class CityService : ICityService
    {
        private const int RandomNamesCount = 25;

        private readonly ICityRepository _cityRepository;
        private readonly IUserService _userService;

        public CityService(
            ICityRepository cityRepository,
            IUserService userService)
        {
            _cityRepository = cityRepository;
            _userService = userService;
        }

        public async Task<City?> GetMyCity(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var myUser = await _userService.GetMyUser(userClaimsPrincipal, cancellationToken);
            if (myUser == null)
                return null;

            return await _cityRepository.FindByUser(myUser.Id, cancellationToken);
        }

        public async Task<string[]> GetRandomCityNames(CancellationToken cancellationToken)
        {
            return await _cityRepository.GetRandomCityNames(RandomNamesCount, cancellationToken);
        }
    }
}