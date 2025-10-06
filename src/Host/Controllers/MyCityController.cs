using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cities;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.MyUsers;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/city")]
    public class MyCityController : Controller
    {
        private readonly ICityService _cityService;

        public MyCityController(
            ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<MyDataResponse<MyCity>> Get(CancellationToken cancellationToken)
        {
            var myCity = await _cityService.GetMyCity(HttpContext.User, cancellationToken);
            return myCity.ToMyDataResponse();
        }

        [HttpGet]
        [Route("get-random-names")]
        public async Task<string[]> GetRandomNames(CancellationToken cancellationToken)
        {
            return await _cityService.GetRandomCityNames(cancellationToken);
        }
    }
}
