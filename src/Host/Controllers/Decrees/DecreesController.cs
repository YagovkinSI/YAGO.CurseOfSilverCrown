using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static YAGO.World.Application.Decrees.Queries.GetDecrees.GetDecreeQueryHandler;

namespace YAGO.World.Host.Controllers.Decrees
{
    [ApiController]
    [Route("api/decrees")]
    public class DecreesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DecreesController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getDecree")]
        public async Task<DecreeDetails> Get(long id, CancellationToken cancellationToken)
        {
            var command = new GetDecreeQuery(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.Decree.ToMyDataResponse();
        }
    }
}
