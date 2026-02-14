using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Decrees;
using YAGO.World.Domain.Decrees;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Decrees;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/decrees")]
    public class DecreesController : ControllerBase
    {
        private readonly IDecreeService _unitService;

        public DecreesController(
            IDecreeService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<DecreeDetails> Get(long id, CancellationToken cancellationToken)
        {
            var unit = await _unitService.GetDecree(id, cancellationToken);
            return unit == null ? throw new YagoNotFoundException(nameof(Decree), id) : unit.ToMyDataResponse();
        }
    }
}
