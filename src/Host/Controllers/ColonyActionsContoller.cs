using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.IssueDecree;
using YAGO.World.Application.Colonies.RunCycle;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/colony-actions")]
    [Authorize]
    public class ColonyActionsContoller : ControllerBase
    {
        private readonly IRunCycleProcessor _runCycleProcessor;
        private readonly IIssueDecreeProcessor _issueDecreeProcessor;

        public ColonyActionsContoller(
            IRunCycleProcessor runCycleProcessor,
            IIssueDecreeProcessor issueDecreeProcessor)
        {
            _runCycleProcessor = runCycleProcessor;
            _issueDecreeProcessor = issueDecreeProcessor;
        }

        [HttpPost("runCycle")]
        public async Task<ApiResponse<EpisodeResponse>> RunCycle(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new RunCycleCommand(userId);
            var result = await _runCycleProcessor.Execute(command, cancellationToken);
            var notification = result.Episode?.ToResponse();
            var myCycle = result.MyCycle?.ToMyCycle();
            var myColony = result.MyColony?.ToMyColony();
            var updatedEntities = new UpdatedEntities(
                myCycle: myCycle,
                myColony: myColony);
            return ApiResponse<EpisodeResponse>.CreateSuccess(notification, updatedEntities);
        }

        [HttpPost("issueDecree")]
        public async Task<ApiResponse<EpisodeResponse>> ConcludeСontract(IssueDecreeRequest сoncludeСontractRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new IssueDecreeCommand(userId, сoncludeСontractRequest.DecreeId);
            var result = await _issueDecreeProcessor.Execute(
                command,
                cancellationToken);
            var myColony = result.MyColony.ToMyColony();
            var updatedEntities = new UpdatedEntities(
                myColony: myColony);
            return ApiResponse<EpisodeResponse>.CreateSuccess(data: null, updatedEntities);
        }
    }
}
