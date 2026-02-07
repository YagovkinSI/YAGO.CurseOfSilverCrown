using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Contracts;
using YAGO.World.Domain.Companies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Units;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/units")]
    public class UnitsController : ControllerBase
    {
        private readonly IContractService _unitService;

        public UnitsController(
            IContractService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<UnitDetails> Get(long id, CancellationToken cancellationToken)
        {
            var unit = await _unitService.GetContract(id, cancellationToken);
            return unit == null ? throw new YagoNotFoundException(nameof(Company), id) : unit.ToMyDataResponse();
        }
    }
}
