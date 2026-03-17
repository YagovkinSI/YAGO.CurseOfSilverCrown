using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/colony")]
    public class MyColonyController : ControllerBase
    {
        private readonly IColonyService _colonyService;

        public MyColonyController(
            IColonyService colonyService)
        {
            _colonyService = colonyService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ApiResponse<MyColony>> Get(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<MyColony>.Empty;

            var userId = User.GetUserId();
            var currentColony = await _colonyService.GetMyColony(userId, cancellationToken);
            return currentColony.ToMyDataResponse();
        }
    }
}
