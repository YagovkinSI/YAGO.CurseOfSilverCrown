using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.Commands.CompleteQuest;
using YAGO.World.Application.Colonies.Commands.DeactivateColony;
using YAGO.World.Application.Colonies.Commands.IssueDecree;
using YAGO.World.Application.Colonies.Queries.GetMyColony;
using YAGO.World.Host.Controllers.Colonies.Models;
using YAGO.World.Host.Controllers.Colonies.MyQuests;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Decrees;
using YAGO.World.Host.Controllers.Episodes;

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
            return (result.Colony?.ToMyColony()).ToApiResponse();
        }

        [HttpPost("issueDecree")]
        public async Task ConcludeСontract(IssueDecreeRequest сoncludeСontractRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new IssueDecreeCommand(userId, сoncludeСontractRequest.DecreeId);
            await _mediator.Send(command, cancellationToken);
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
        public async Task<ApiResponse<MyQuest>> GetColonyQuest(string id, CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<MyQuest>.Empty;

            var userId = User.GetUserId();
            var command = new GetColonyQuestQuery(userId, id);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.ColonyQuest?.ToMyQuest()).ToApiResponse();
        }

        [Authorize]
        [HttpPost("completeQuest")]
        public async Task<EpisodeResponse> CompleteQuest(CompleteQuestRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new CompleteQuestCommand(userId, request.Id, request.DilemmaResolving);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToResponse();
        }
    }
}
