using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.GetColonyWithDetails;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Cycles;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers.Colonies
{
    [ApiController]
    [Route("api/me/colony")]
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
            if (!User.IsAuthenticated())
                return MyDataResponse<MyColony>.NotAuthorized;

            var userId = User.GetUserId();
            var command = new GetColonyWithDetailsCommand(userId);
            var currentColony = await _colonyWithDetailsProvider.Get(command, cancellationToken);
            return currentColony.ToMyDataResponse();
        }
    }
}
