using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Council.Queries.GetCouncilPositions;
using YAGO.World.Host.Controllers.Common.Extensions;

namespace YAGO.World.Host.Controllers.Council
{
    [ApiController]
    [Route("api/council")]
    public class CouncilsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CouncilsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("getCouncilPositions")]
        public async Task<IReadOnlyList<CouncilPositionResponse>> GetCouncilPositions(
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var query = new GetCouncilPositionsQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);
            return result.Positions.ToResponse();
        }
    }
}