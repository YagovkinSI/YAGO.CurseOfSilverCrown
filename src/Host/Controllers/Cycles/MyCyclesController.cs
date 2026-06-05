using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles.Queries.GetMyCycle;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Episodes;
using static YAGO.World.Application.Cycles.Commands.RunCycle.RunCycleCommandHandler;
using static YAGO.World.Application.Colonies.Commands.CompleteEvent.CompleteEventCommandHandler;

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
            var command = new GetMyCycleQuery(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.Cycle.ToMyDataResponse();
        }

        [Authorize]
        [HttpPost("runCycle")]
        public async Task<MyCycle> RunCycle(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new RunCycleCommand(userId);
            var result = await _mediator.Send(command, cancellationToken);
            var myCycle = result.Cycle.ToMyCycle();
            return myCycle;
        }

        [HttpPost("setChoice")]
        public async Task SetChoice(SetChoiceRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new CompleteEventCommand(userId, request.EventId, request.DilemmaResolving);
            await _mediator.Send(command, cancellationToken);
        }
    }
}
