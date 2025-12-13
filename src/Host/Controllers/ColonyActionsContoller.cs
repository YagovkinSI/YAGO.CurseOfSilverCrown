using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.AttackColony;
using YAGO.World.Application.Colonies.BuyBuilding;
using YAGO.World.Application.Colonies.RunCycle;
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
        private readonly IAttackColonyProcessor _attackColonyProcessor;
        private readonly IBuyBuildingProcessor _buyBuildingProcessor;

        public ColonyActionsContoller(
            IRunCycleProcessor runCycleProcessor,
            IAttackColonyProcessor attackColonyProcessor,
            IBuyBuildingProcessor buyBuildingProcessor)
        {
            _runCycleProcessor = runCycleProcessor;
            _attackColonyProcessor = attackColonyProcessor;
            _buyBuildingProcessor = buyBuildingProcessor;
        }


        [HttpPost("runCycle")]
        public async Task<ColonyActionResponse> RunCycle(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new RunCycleCommand(userId);
            var result = await _runCycleProcessor.Execute(command, cancellationToken);
            var myCycle = result.Cycle.ToMyCycle();
            var myColony = result.ColonyWithShipAndBuildings.ToMyColony();
            var updatedEntities = new UpdatedColonyEntities(
                myCycle: myCycle,
                myColony: myColony);
            return new ColonyActionResponse(notification: null, updatedEntities);
        }

        [HttpPost("attackColony")]
        public async Task<ColonyActionResponse> AttackColony(AttackColonyRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new AttackColonyCommand(userId, request.TargetColonyId, request.PrizeType);
            var result = await _attackColonyProcessor.Execute(command, cancellationToken);
            var myCycle = result.MyCycle.ToMyCycle();
            var myColony = result.MyColony.ToMyColony();
            var otherColonies = result.OtherColonies
                .Select(x => x.ToDetails())
                .ToArray();
            var updatedEntities = new UpdatedColonyEntities(
                myCycle: myCycle,
                myColony: myColony,
                otherColonies: otherColonies);
            return new ColonyActionResponse(notification: null, updatedEntities);
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
