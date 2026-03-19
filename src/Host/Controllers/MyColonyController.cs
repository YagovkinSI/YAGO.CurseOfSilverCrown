using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/colony")]
    public class MyColonyController : ControllerBase
    {
        private readonly IGetMyColonyProcessor _getMyColonyProcessor;

        public MyColonyController(
            IGetMyColonyProcessor getMyColonyProcessor)
        {
            _getMyColonyProcessor = getMyColonyProcessor;
        }

        [HttpGet("getMyColony")]
        public async Task<ApiResponse<MyColony>> GetMyColony(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<MyColony>.Empty;

            var userId = User.GetUserId();
            var command = new GetMyColonyCommand(userId);
            var result = await _getMyColonyProcessor.Execute(command, cancellationToken);
            return result.Colony.ToApiResponse();
        }
    }
}
