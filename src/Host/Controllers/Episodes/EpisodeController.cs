using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Common;
using static YAGO.World.Application.Cycles.Commands.RunCycle.RunCycleCommandHandler;

namespace YAGO.World.Host.Controllers.Episodes
{
    [ApiController]
    [Route("api/episode")]
    public class EpisodeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EpisodeController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        [Route("action")]
        public async Task<EpisodeResponse> Get(EpisodeActionRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            switch (request.ActionName)
            {
                case EpisodeActionNames.RunCycle:
                    var runCycleCommand = new RunCycleCommand(userId);
                    var result = await _mediator.Send(runCycleCommand, cancellationToken);
                    return result.Episode.ToResponse();
                default:
                    throw new YagoUnknownTypeException(request.ActionName);
            }
        }
    }
}
