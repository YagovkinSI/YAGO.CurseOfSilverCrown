using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.RunCycle;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/colony-actions")]
    public class ColonyActionsContoller : ControllerBase
    {
        private readonly IRunCycleProcessor _runCycleProcessor;

        public ColonyActionsContoller(
            IRunCycleProcessor runCycleProcessor)
        {
            _runCycleProcessor = runCycleProcessor;
        }


        [HttpPost("runCycle")]
        public async Task<MyDataResponse<MyCycle>> RunCycle(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new RunCycleCommand(userId);
            var result = await _runCycleProcessor.Execute(command, cancellationToken);
            return result.Cycle.ToMyDataResponse();
        }
    }
}
