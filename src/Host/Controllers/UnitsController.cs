using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Units;
using YAGO.World.Host.Controllers.Buildings;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Units;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/units")]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(
            IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<UnitDetails> Get(long id, CancellationToken cancellationToken)
        {
            var unit = await _unitService.GetUnit(id, cancellationToken);
            return unit == null ? throw new YagoNotFoundException(nameof(Contract), id) : unit.ToMyDataResponse();
        }
    }
}
