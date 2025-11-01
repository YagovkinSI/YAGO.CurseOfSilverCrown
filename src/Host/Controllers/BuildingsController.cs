using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Buildings;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/buildings")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingsController(
            IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<BuildingDetails> Get(long id, CancellationToken cancellationToken)
        {
            var building = await _buildingService.GetBuilding(id, cancellationToken);
            return building == null ? throw new YagoNotFoundException(nameof(Building), id) : building.ToMyDataResponse();
        }
    }
}
