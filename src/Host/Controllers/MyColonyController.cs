using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.MyUsers;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/colony")]
    public class MyColonyController : Controller
    {
        private readonly IColonyService _colonyService;

        public MyColonyController(
            IColonyService colonyService)
        {
            _colonyService = colonyService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<MyDataResponse<MyColony>> Get(CancellationToken cancellationToken)
        {
            var currentColony = await _colonyService.GetMyColony(HttpContext.User, cancellationToken);
            return currentColony.ToMyDataResponse();
        }

        [HttpPost("createColony")]
        public async Task<MyDataResponse<MyColony>> CreateColony(CreateColonyRequest createColonyRequest, CancellationToken cancellationToken)
        {
            if (createColonyRequest.PresetType == ColonyPresetType.Unknown)
                throw new YagoUnknownTypeException(nameof(ColonyPresetType));

            var currentColony = await _colonyService.CreateColony(HttpContext.User, createColonyRequest.Name, createColonyRequest.PresetType, cancellationToken);
            return currentColony.ToMyDataResponse();
        }
    }
}
