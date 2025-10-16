using Microsoft.AspNetCore.Mvc;
using System;
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
    public class MyCycleController : Controller
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
                var currentColony = await _cycleService.GetMyLastCycle(HttpContext.User, cancellationToken);
                return currentColony.ToMyDataResponse();
            }
            catch (YagoNotAuthorizedException)
            {
                return MyDataResponse<MyCycle>.NotAuthorized;
            }

        }

        [HttpPost("runCycle")]
        public Task<MyDataResponse<MyCycle>> RunCycle(CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Данный функциоанал пока в разработке.");
        }
    }
}
