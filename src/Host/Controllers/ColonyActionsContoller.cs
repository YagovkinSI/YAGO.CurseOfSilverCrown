using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
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

        public ColonyActionsContoller(
            IRunCycleProcessor runCycleProcessor)
        {
            _runCycleProcessor = runCycleProcessor;
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
    }
}
