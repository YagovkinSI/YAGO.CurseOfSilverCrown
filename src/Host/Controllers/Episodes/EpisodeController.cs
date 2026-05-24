using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;
using static YAGO.World.Application.Cycles.Commands.RunCycle.RunCycleCommandHandler;
using static YAGO.World.Application.Cycles.Commands.SetChoice.SetChoiceCommandHandler;

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
        public async Task<ApiResponse<EpisodeResponse>> Get(EpisodeActionRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            switch (request.ActionName)
            {
                case EpisodeActionNames.RunCycle:
                    var runCycleCommand = new RunCycleCommand(userId);
                    var runCycleResult = await _mediator.Send(runCycleCommand, cancellationToken);
                    return runCycleResult.Episode.ToResponse().ToApiResponse();
                case EpisodeActionNames.SetChoice:
                    var setChoiceCommand = new SetChoiceCommand(userId, request.ActionParameters);
                    await _mediator.Send(setChoiceCommand, cancellationToken);
                    return ApiResponse<EpisodeResponse>.Empty;
                default:
                    throw new YagoUnknownTypeException(request.ActionName);
            }
        }
    }
}
