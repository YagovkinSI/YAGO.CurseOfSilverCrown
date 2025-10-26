using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
    [Authorize]
    public class MyColonyController : ControllerBase
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
            try
            {
                var userId = User.GetUserId();
                var currentColony = await _colonyService.GetMyColonyWithShipAndBuildings(userId, cancellationToken);
                return currentColony.ToMyDataResponse();
            }
            catch (YagoNotAuthorizedException)
            {
                return MyDataResponse<MyColony>.NotAuthorized;
            }

        }

        [HttpPost("createColony")]
        public async Task<MyDataResponse<MyColony>> CreateColony(CreateColonyRequest createColonyRequest, CancellationToken cancellationToken)
        {
            if (createColonyRequest.PresetType == ColonyPresetType.Unknown)
                throw new YagoUnknownTypeException(nameof(ColonyPresetType));

            try
            {
                var userId = User.GetUserId();
                var currentColony = await _colonyService.CreateColony(
                    userId,
                    createColonyRequest.Name,
                    createColonyRequest.PresetType,
                    cancellationToken);
                return currentColony.ToMyDataResponse();
            }
            catch (YagoNotAuthorizedException)
            {
                return MyDataResponse<MyColony>.NotAuthorized;
            }
        }

        [HttpPost("buyBuilding")]
        public async Task<MyDataResponse<MyColony>> BuyBuilding(BuyBuildingRequest buyBuildingRequest, CancellationToken cancellationToken)
        {
            if (buyBuildingRequest.BuildingId < 1)
                throw new YagoException("Не валидный запрос. 'BuildingId' не может быть меньше 1.");

            try
            {
                var userId = User.GetUserId();
                var currentColony = await _colonyService.BuyBuilding(
                    userId,
                    buyBuildingRequest.BuildingId,
                    cancellationToken);
                return currentColony.ToMyDataResponse();
            }
            catch (YagoNotAuthorizedException)
            {
                return MyDataResponse<MyColony>.NotAuthorized;
            }
        }
    }
}
