using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Events.Commands;
using YAGO.World.Application.Events.Queries;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Host.Controllers.Common.Models;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events.Models;

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
        [HttpGet("getColonyEvent")]
        public async Task<ApiResponse<ColonyEventPrivate>> GetColonyEvent(long id, CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<ColonyEventPrivate>.Empty;

            var userId = User.GetUserId();
            var command = new GetColonyEventQuery(userId, id);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.ColonyEvent?.ToResponse()).ToApiResponse();
        }

        [Authorize]
        [HttpPost("setRead")]
        public async Task SetRead(SetReadRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new SetReadCommand(userId, request.ColonyEventId);
            await _mediator.Send(command, cancellationToken);
        }

        [Authorize]
        [HttpPost("completeEvent")]
        public async Task<ApiResponse<EventResultSlideResponse>> CompleteEvent(CompleteQuestRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new CompleteEventCommand(userId, request.ColonyEventId, request.DilemmaResolving);
            var result = await _mediator.Send(command, cancellationToken);
            return result.EventResult == null
                ? ApiResponse<EventResultSlideResponse>.Empty
                : result.EventResult.ToResponse().ToApiResponse();
        }
    }
}
