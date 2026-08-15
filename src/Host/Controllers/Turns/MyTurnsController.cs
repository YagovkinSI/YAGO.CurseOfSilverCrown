using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Turns.Commands.RunTurn;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Turns
{
    [ApiController]
    [Route("api/me/turn")]
    public class MyTurnController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MyTurnController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("runTurn")]
        public async Task<ApiResponse<EventResultSlideResponse>> RunTurn(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new RunTurnCommand(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.EventResult.ToResponse().ToApiResponse();
        }
    }
}
