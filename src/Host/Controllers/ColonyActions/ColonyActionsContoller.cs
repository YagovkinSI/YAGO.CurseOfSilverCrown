using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.CreateColony;
using YAGO.World.Application.Colonies.DeactivateColony;
using YAGO.World.Application.Colonies.IssueDecree;
using YAGO.World.Application.Colonies.RunCycle;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.ColonyActions
{
    [ApiController]
    [Route("api/colony-actions")]
    [Authorize]
    public class ColonyActionsContoller : ControllerBase
    {
        private readonly IRunCycleProcessor _runCycleProcessor;
        private readonly IIssueDecreeProcessor _issueDecreeProcessor;
        private readonly ICreateColonyProcessor _createColonyProcessor;
        private readonly IDeactivateColonyProcessor _deactivateColonyProcessor;

        public ColonyActionsContoller(
            IRunCycleProcessor runCycleProcessor,
            IIssueDecreeProcessor issueDecreeProcessor,
            ICreateColonyProcessor createColonyProcessor,
            IDeactivateColonyProcessor deactivateColonyProcessor)
        {
            _runCycleProcessor = runCycleProcessor;
            _issueDecreeProcessor = issueDecreeProcessor;
            _createColonyProcessor = createColonyProcessor;
            _deactivateColonyProcessor = deactivateColonyProcessor;
        }

        [HttpPost("createColony")]
        public async Task<ColonyActionResponse> CreateColony(CreateColonyRequest createColonyRequest, CancellationToken cancellationToken)
        {
            if (createColonyRequest.PresetType == CodeOfLaws.Unknown)
                throw new YagoUnknownTypeException(nameof(CodeOfLaws));

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
            return new ColonyActionResponse(Episode: null, updatedEntities);
        }

        [HttpPost("runCycle")]
        public async Task<ColonyActionResponse> RunCycle(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new RunCycleCommand(userId);
            var result = await _runCycleProcessor.Execute(command, cancellationToken);
            var notification = result.Episode?.ToResponse();
            var myCycle = result.MyCycle?.ToMyCycle();
            var myColony = result.MyColony?.ToMyColony();
            var updatedEntities = new UpdatedColonyEntities(
                myCycle: myCycle,
                myColony: myColony);
            return new ColonyActionResponse(notification, updatedEntities);
        }

        [HttpPost("issueDecree")]
        public async Task<ColonyActionResponse> ConcludeСontract(IssueDecreeRequest сoncludeСontractRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new IssueDecreeCommand(userId, сoncludeСontractRequest.DecreeId);
            var result = await _issueDecreeProcessor.Execute(
                command,
                cancellationToken);
            var myColony = result.MyColony.ToMyColony();
            var updatedEntities = new UpdatedColonyEntities(
                myColony: myColony);
            return new ColonyActionResponse(Episode: null, updatedEntities);
        }

        [HttpPost("deactivateColony")]
        public async Task<ColonyActionResponse> DeactivateColony(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new DeactivateColonyCommand(
                userId);
            await _deactivateColonyProcessor.Execute(
                command,
                cancellationToken);
            var updatedEntities = new UpdatedColonyEntities();
            return new ColonyActionResponse(Episode: null, updatedEntities);
        }
    }
}
