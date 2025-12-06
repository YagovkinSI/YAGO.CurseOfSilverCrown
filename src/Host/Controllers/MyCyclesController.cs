using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/cycle")]
    [Authorize]
    public class MyCycleController : ControllerBase
    {
        private readonly ICycleService _cycleService;

        public MyCycleController(
            ICycleService cycleService)
        {
            _cycleService = cycleService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<MyDataResponse<MyCycle>> Get(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var currentCycle = await _cycleService.GetMyLastCycle(userId, cancellationToken);
            return currentCycle.ToMyDataResponse();
        }

        [HttpPost("runCycle")]
        public async Task<MyDataResponse<MyCycle>> RunCycle(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var currentCycle = await _cycleService.RunCycle(userId, cancellationToken);
            return currentCycle.ToMyDataResponse();
        }

        [HttpPost("attackColony")]
        public async Task<MyDataResponse<MyCycle>> AttackColony(AttackColonyRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var currentCycle = await _cycleService.AttackColony(
                userId,
                request.TargetColonyId,
                request.PrizeType,
                cancellationToken);
            return currentCycle.ToMyDataResponse();
        }
    }
}
