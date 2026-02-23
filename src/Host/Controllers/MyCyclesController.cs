using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/cycle")]    
    public class MyCycleController : ControllerBase
    {
        private readonly ICycleProvider _cycleProvider;

        public MyCycleController(
            ICycleProvider cycleService)
        {
            _cycleProvider = cycleService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<MyDataResponse<MyCycle>> Get(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return MyDataResponse<MyCycle>.NotAuthorized;

            var userId = User.GetUserId();
            var command = new GetCycleCommand(userId);
            var currentCycle = await _cycleProvider.Execute(command, cancellationToken);
            return currentCycle.ToMyDataResponse();
        }
    }
}
