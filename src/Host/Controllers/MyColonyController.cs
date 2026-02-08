using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.GetColonyWithDetails;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/colony")]
    [Authorize]
    public class MyColonyController : ControllerBase
    {
        private readonly IColonyWithDetailsProvider _colonyWithDetailsProvider;

        public MyColonyController(
            IColonyWithDetailsProvider colonyWithDetailsProvider)
        {
            _colonyWithDetailsProvider = colonyWithDetailsProvider;
        }

        [HttpGet]
        [Route("get")]
        public async Task<MyDataResponse<MyColony>> Get(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new GetColonyWithDetailsCommand(userId);
            var currentColony = await _colonyWithDetailsProvider.Execute(command, cancellationToken);
            return currentColony.ToMyDataResponse();
        }
    }
}
