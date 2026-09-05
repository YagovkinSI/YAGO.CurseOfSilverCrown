using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Reforms.Queries.GetReform;
using YAGO.World.Application.Reforms.Queries.GetReforms;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common.Extensions;

namespace YAGO.World.Host.Controllers.Reforms
{
    [ApiController]
    [Route("api/reforms")]
    public class ReformsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReformsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("getReforms")]
        public async Task<IReadOnlyList<ReformSummary>> GetReforms(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new GetReformsQuery(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ReformDtos.ToResponse();
        }

        [HttpGet]
        [Authorize]
        [Route("getReform")]
        public async Task<ReformDetails> Get(string code, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new GetReformQuery(userId, code);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ColonyState.ToReformDetails(result.ReformDto);
        }
    }
}
