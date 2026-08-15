using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.Commands.CompleteEvent;
using YAGO.World.Application.Colonies.Commands.CreateColony;
using YAGO.World.Application.Colonies.Commands.RunTurn;
using YAGO.World.Application.Colonies.Commands.SetReform;
using YAGO.World.Application.Colonies.Queries.GetColonyPrivate;
using YAGO.World.Application.Colonies.Queries.GetColonyQuest;
using YAGO.World.Application.Colonies.Queries.GetPaginatedColonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events.Models;
using YAGO.World.Host.Controllers.Reforms;

namespace YAGO.World.Host.Controllers.Colonies
{
    [ApiController]
    [Route("api/colonies")]
    public class ColoniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColoniesController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getMyColony")]
        public async Task<ApiResponse<ColonyPrivate>> GetMyColony(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<ColonyPrivate>.Empty;

            var userId = User.GetUserId();
            var command = new GetColonyPrivateQuery(userId);
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

        [HttpGet]
        [Route("getColonyRaiting")]
        public async Task<PaginatedResponse<ColonyDetails>> GetColonyRaiting(int page, CancellationToken cancellationToken)
        {
            var command = new GetPaginatedColoniesQuery(page);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ColoniesPaginated.ToPaginatedResponse();
        }

        [Authorize]
        [HttpPost]
        [Route("createColony")]
        public async Task<ApiResponse<ColonyPrivate>> CreateColony(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new CreateColonyCommand(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.Colony?.ToMyColony(result.ColonyEvents)).ToApiResponse();
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
