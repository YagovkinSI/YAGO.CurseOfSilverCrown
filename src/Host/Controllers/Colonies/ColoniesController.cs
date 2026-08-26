using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.Commands;
using YAGO.World.Application.Colonies.Queries;
using YAGO.World.Application.Statistics.Queries;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Host.Controllers.Common.Models;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events;

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

        [Authorize]
        [HttpGet("getMyColony")]
        public async Task<ApiResponse<ColonyPrivate>> GetMyColony(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new GetColonyPrivateQuery(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.ColonyPrivate?.ToResponse()).ToApiResponse();
        }

        [Authorize]
        [HttpGet("getStatistics")]
        public async Task<StatisticsResponse> GetStatistics(string statisticType, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var statisticTypeEnum = statisticType.ToStatisticType();
            var command = new GetStatisticsQuery(userId, statisticTypeEnum);
            var result = await _mediator.Send(command, cancellationToken);
            return result.Composition.ToResponse();
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
            return (result.ColonyPrivate?.ToResponse()).ToApiResponse();
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
