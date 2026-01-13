using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.BuyBuilding;
using YAGO.World.Application.Colonies.CreateColony;
using YAGO.World.Application.Colonies.RunCycle;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.ColonyActions;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/colony-actions")]
    [Authorize]
    public class ColonyActionsContoller : ControllerBase
    {
        private readonly IRunCycleProcessor _runCycleProcessor;
        private readonly IBuyBuildingProcessor _buyBuildingProcessor;
        private readonly ICreateColonyProcessor _createColonyProcessor;

        public ColonyActionsContoller(
            IRunCycleProcessor runCycleProcessor,
            IBuyBuildingProcessor buyBuildingProcessor,
            ICreateColonyProcessor createColonyProcessor)
        {
            _runCycleProcessor = runCycleProcessor;
            _buyBuildingProcessor = buyBuildingProcessor;
            _createColonyProcessor = createColonyProcessor;
        }

        [HttpPost("createColony")]
        public async Task<ColonyActionResponse> CreateColony(CreateColonyRequest createColonyRequest, CancellationToken cancellationToken)
        {
            if (createColonyRequest.PresetType == ColonyPresetType.Unknown)
                throw new YagoUnknownTypeException(nameof(ColonyPresetType));

            var userId = User.GetUserId();
            var command = new CreateColonyCommand(
                userId,
                createColonyRequest.Name,
                createColonyRequest.PresetType);
            var result = await _createColonyProcessor.Execute(
                command,
                cancellationToken);
            var myColony = result.MyColony.ToMyColony();
            var updatedEntities = new UpdatedColonyEntities(
                myColony: myColony);
            return new ColonyActionResponse(notification: null, updatedEntities);
        }

        [HttpPost("runCycle")]
        public async Task<ColonyActionResponse> RunCycle(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new RunCycleCommand(userId);
            var result = await _runCycleProcessor.Execute(command, cancellationToken);
            var notification = result.Notification.ToResponse();
            var myCycle = result.MyCycle.ToMyCycle();
            var myColony = result.MyColony.ToMyColony();
            var updatedEntities = new UpdatedColonyEntities(
                myCycle: myCycle,
                myColony: myColony);
            return new ColonyActionResponse(notification, updatedEntities);
        }

        [HttpPost("buyBuilding")]
        public async Task<ColonyActionResponse> BuyBuilding(BuyBuildingRequest buyBuildingRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new BuyBuildingCommand(userId, buyBuildingRequest.BuildingId);
            var result = await _buyBuildingProcessor.Execute(
                command,
                cancellationToken);
            var myColony = result.MyColony.ToMyColony();
            var updatedEntities = new UpdatedColonyEntities(
                myColony: myColony);
            return new ColonyActionResponse(notification: null, updatedEntities);
        }
    }
}
