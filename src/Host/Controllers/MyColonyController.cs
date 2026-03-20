using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.Commands.CreateColony;
using YAGO.World.Application.Colonies.Commands.DeactivateColony;
using YAGO.World.Application.Colonies.Queries.GetMyColony;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers
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
            return result.Colony.ToApiResponse();
        }

        [HttpPost("createColony")]
        public async Task CreateColony(CreateColonyRequest createColonyRequest, CancellationToken cancellationToken)
        {
            if (createColonyRequest.PresetType == CodeOfLaws.Unknown)
                throw new YagoUnknownTypeException(nameof(CodeOfLaws));

            var userId = User.GetUserId();
            var command = new CreateColonyCommand(
                userId,
                createColonyRequest.Name,
                createColonyRequest.PresetType);
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
    }
}
