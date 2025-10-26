using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.MyUsers;

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
            try
            {
                var userId = User.GetUserId();
                var currentColony = await _cycleService.GetMyLastCycle(userId, cancellationToken);
                return currentColony.ToMyDataResponse();
            }
            catch (YagoNotAuthorizedException)
            {
                return MyDataResponse<MyCycle>.NotAuthorized;
            }

        }

        [HttpPost("runCycle")]
        public async Task<MyDataResponse<MyCycle>> RunCycle(CancellationToken cancellationToken)
        {
            try
            {
                var userId = User.GetUserId();
                var currentColony = await _cycleService.RunCycle(userId, cancellationToken);
                return currentColony.ToMyDataResponse();
            }
            catch (YagoNotAuthorizedException)
            {
                return MyDataResponse<MyCycle>.NotAuthorized;
            }
        }
    }
}
