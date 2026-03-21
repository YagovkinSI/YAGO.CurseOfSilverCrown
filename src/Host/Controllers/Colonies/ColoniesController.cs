using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.GetPaginatedColonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Host.Controllers.Colonies.Models;

namespace YAGO.World.Host.Controllers.Colonies
{
    [ApiController]
    [Route("api/colonies")]
    public class ColoniesController : ControllerBase
    {
        private readonly IPaginatedColoniesProvider _paginatedColoniesProvider;

        public ColoniesController(
            IPaginatedColoniesProvider paginatedColoniesProvider)
        {
            _paginatedColoniesProvider = paginatedColoniesProvider;
        }

        [HttpGet]
        [Route("getColonyRaiting")]
        public async Task<PaginatedResponse<ColonyDetails>> GetColonyRaiting(int page, CancellationToken cancellationToken)
        {
            var command = new GetPaginatedColoniesCommand(page);
            var result = await _paginatedColoniesProvider.Get(command, cancellationToken);
            return result.ToPaginatedResponse();
        }
    }
}
