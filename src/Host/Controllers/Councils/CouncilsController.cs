using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Councils.Queries.GetCouncilPositions;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Host.Controllers.Common.Models;

namespace YAGO.World.Host.Controllers.Councils
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
        public async Task<ApiResponse<IReadOnlyList<CouncilPositionResponse>>> GetCouncilPositions(
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var query = new GetCouncilPositionsQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);
            return ApiResponse<IReadOnlyList<CouncilPositionResponse>>.CreateSuccess(
                result.Positions.ToResponse());
        }
    }
}
