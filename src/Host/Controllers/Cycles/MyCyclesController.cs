using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles.Commands.GetMyCycle;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Episodes;
using static YAGO.World.Application.Cycles.Commands.RunCycle.RunCycleCommandHandler;

namespace YAGO.World.Host.Controllers.Cycles
{
    [ApiController]
    [Route("api/me/cycle")]
    public class MyCycleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MyCycleController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getMyCycle")]
        public async Task<ApiResponse<MyCycle>> Get(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<MyCycle>.Empty;

            var userId = User.GetUserId();
            var command = new GetMyCycleCommand(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.Cycle.ToMyDataResponse();
        }

        [HttpPost("runCycle")]
        public async Task<ApiResponse<EpisodeResponse>> RunCycle(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new RunCycleCommand(userId);
            var result = await _mediator.Send(command, cancellationToken);
            var notification = result.Episode?.ToResponse(result.IsCycleCompleted);
            return ApiResponse<EpisodeResponse>.CreateSuccess(notification);
        }
    }
}
