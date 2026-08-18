using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Reforms.Queries.GetReform;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Decrees;

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
        [Route("getReform")]
        public async Task<ReformDetails> Get(long id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new GetReformQuery(userId, id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ColonyState.ToReformDetails(result.ReformDto);
        }
    }
}
