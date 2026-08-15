using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Events.Models;
using YAGO.World.Application.Events.Commands;

namespace YAGO.World.Host.Controllers.Events
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("setRead")]
        public async Task SetRead(SetReadRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new SetReadCommand(userId, request.EventId);
            await _mediator.Send(command, cancellationToken);
        }
    }
}
