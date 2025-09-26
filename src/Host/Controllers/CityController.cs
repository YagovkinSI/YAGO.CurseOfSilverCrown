using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cities.Interfaces;
using YAGO.World.Host.Controllers.Models.Cities;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/city")]
    public class CityController : Controller
    {
        private readonly ICurrentCityService _currentCityService;

        public CityController(
            ICurrentCityService currentCityService)
        {
            _currentCityService = currentCityService;
        }

        [HttpGet]
        [Route("getCurrentCity")]
        public async Task<CurrentCityResponse> GetCurrentCity(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentCity = await _currentCityService.GetCurrentCity(HttpContext.User, cancellationToken);
            return new CurrentCityResponse(currentCity);
        }
    }
}
