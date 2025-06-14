using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YAGO.World.Application.Factions;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Factions.Extensions;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/factions")]
    public class FactionsController : Controller
    {
        private readonly FactionService _factionService;

        public FactionsController(FactionService factionService)
        {
            _factionService = factionService;
        }

        public Task<ListItem[]> Index(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortBy = "vassalCount",
            [FromQuery] string sortOrder = "asc"
        )
        {
            var factionOrderBy = sortBy.ToFactionOrderBy();
            var useOrderByDescending = sortOrder?.ToLower() == "desc";
            return _factionService.GetFactionList(page, pageSize, factionOrderBy, useOrderByDescending);
        }
    }
}
