using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.IssueDecree;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Decrees;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Colonies
{
    [ApiController]
    [Route("api/colony-actions")]
    [Authorize]
    public class ColonyActionsContoller : ControllerBase
    {
        private readonly IIssueDecreeProcessor _issueDecreeProcessor;

        public ColonyActionsContoller(
            IIssueDecreeProcessor issueDecreeProcessor)
        {
            _issueDecreeProcessor = issueDecreeProcessor;
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
