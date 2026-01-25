using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.ConcludeContract;
using YAGO.World.Application.Colonies.CreateColony;
using YAGO.World.Application.Colonies.DeactivateColony;
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
        private readonly IConcludeContractProcessor _сoncludeСontractProcessor;
        private readonly ICreateColonyProcessor _createColonyProcessor;
        private readonly IDeactivateColonyProcessor _deactivateColonyProcessor;

        public ColonyActionsContoller(
            IRunCycleProcessor runCycleProcessor,
            IConcludeContractProcessor сoncludeСontractProcessor,
            ICreateColonyProcessor createColonyProcessor,
            IDeactivateColonyProcessor deactivateColonyProcessor)
        {
            _runCycleProcessor = runCycleProcessor;
            _сoncludeСontractProcessor = сoncludeСontractProcessor;
            _createColonyProcessor = createColonyProcessor;
            _deactivateColonyProcessor = deactivateColonyProcessor;
        }

        [HttpPost("createColony")]
        public async Task<ColonyActionResponse> CreateColony(CreateColonyRequest createColonyRequest, CancellationToken cancellationToken)
        {
            if (createColonyRequest.PresetType == GavernorType.Unknown)
                throw new YagoUnknownTypeException(nameof(GavernorType));

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

        [HttpPost("concludeContract")]
        public async Task<ColonyActionResponse> ConcludeСontract(ConcludeСontractRequest сoncludeСontractRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new ConcludeContractCommand(userId, сoncludeСontractRequest.ContractId);
            var result = await _сoncludeСontractProcessor.Execute(
                command,
                cancellationToken);
            var myColony = result.MyColony.ToMyColony();
            var updatedEntities = new UpdatedColonyEntities(
                myColony: myColony);
            return new ColonyActionResponse(notification: null, updatedEntities);
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
            return new ColonyActionResponse(notification: null, updatedEntities);
        }
    }
}
