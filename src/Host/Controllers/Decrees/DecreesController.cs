using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Decrees;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Host.Controllers.Decrees
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
