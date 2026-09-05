using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.Commands;
using YAGO.World.Application.Events.Commands;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Host.Controllers.Common.Models;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events;
using YAGO.World.Host.Controllers.GameActions.Models;

namespace YAGO.World.Host.Controllers.GameActions
{
    [ApiController]
    [Route("api/gameActions")]
    public class GameActionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameActionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("useAction")]
        public async Task<ApiResponse<EventResultSlideResponse>> UseAction(
            UseActionRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            return request.Type switch
            {
                GameActionType.Event => await UseEvent(userId, request, cancellationToken),
                GameActionType.Reform => await UseReform(userId, request, cancellationToken),
                GameActionType.HireAdvisor => throw new System.NotImplementedException(),
                GameActionType.EndTurn => throw new System.NotImplementedException(),
                _ => throw new System.NotImplementedException(),
            };
        }

        private async Task<ApiResponse<EventResultSlideResponse>> UseEvent(
            long userId, UseActionRequest request, CancellationToken cancellationToken)
        {
            var command = new CompleteEventCommand(
                userId,
                long.Parse(request.Code!),
                request.Value ?? string.Empty);
            var result = await _mediator.Send(command, cancellationToken);
            return result.EventResult == null
                ? ApiResponse<EventResultSlideResponse>.Empty
                : result.EventResult.ToResponse().ToApiResponse();
        }

        private async Task<ApiResponse<EventResultSlideResponse>> UseReform(
            long userId, UseActionRequest request, CancellationToken cancellationToken)
        {
            var command = new SetReformCommand(
                userId,
                request.Code!,
                request.Value ?? string.Empty);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ActionResult.ToResponse().ToApiResponse();
        }
    }
}