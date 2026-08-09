using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.Commands.DeactivateColony;
using YAGO.World.Application.Colonies.Commands.SetReform;
using YAGO.World.Application.Colonies.Queries.GetColonyQuest;
using YAGO.World.Application.Colonies.Queries.GetMyColony;
using YAGO.World.Host.Controllers.Colonies.Models;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Reforms;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events.Models;
using static YAGO.World.Application.Colonies.Commands.CompleteEvent.CompleteEventCommandHandler;

namespace YAGO.World.Host.Controllers.Colonies
{
    [ApiController]
    [Route("api/me/colony")]
    public class MyColonyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MyColonyController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getMyColony")]
        public async Task<ApiResponse<MyColony>> GetMyColony(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<MyColony>.Empty;

            var userId = User.GetUserId();
            var command = new GetMyColonyQuery(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.Colony?.ToMyColony(result.ColonyEvents)).ToApiResponse();
        }

        [HttpPost("issueReform")]
        public async Task<ApiResponse<EventResultSlideResponse>> ConcludeСontract(IssueReformRequest сoncludeСontractRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new SetReformCommand(userId, сoncludeСontractRequest.ReformId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.EventResult.ToResponse().ToApiResponse();
        }

        [HttpPost("deactivateColony")]
        public async Task DeactivateColony(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new DeactivateColonyCommand(
                userId);
            await _mediator.Send(command, cancellationToken);
        }

        [HttpGet("getColonyQuest")]
        public async Task<ApiResponse<ColonyEventResponse>> GetColonyQuest(string id, CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<ColonyEventResponse>.Empty;

            var userId = User.GetUserId();
            var command = new GetColonyEventQuery(userId, id);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.ColonyEvent?.ToMyQuest()).ToApiResponse();
        }

        [Authorize]
        [HttpPost("completeQuest")]
        public async Task<ApiResponse<EventResultSlideResponse>> CompleteQuest(CompleteQuestRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new CompleteEventCommand(userId, request.Id, request.DilemmaResolving);
            var result = await _mediator.Send(command, cancellationToken);
            return result.EventResult == null ? ApiResponse<EventResultSlideResponse>.Empty : result.EventResult.ToResponse().ToApiResponse();
        }
    }
}
