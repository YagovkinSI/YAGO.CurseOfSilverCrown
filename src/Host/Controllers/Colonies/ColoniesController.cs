using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.Commands.CreateColony;
using YAGO.World.Application.Colonies.Queries.GetPaginatedColonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Host.Controllers.Colonies.Models;
using YAGO.World.Host.Controllers.Common;

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
        public async Task<ApiResponse<MyColony>> CreateColony(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new CreateColonyCommand(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.Colony?.ToMyColony(result.ColonyEvents)).ToApiResponse();
        }
    }
}
