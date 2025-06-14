using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YAGO.World.Host.Constants;
using YAGO.World.Host.Extensions;
using YAGO.World.Host.Models;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/factions")]
    public class FactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FactionOnList[]> Index(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortBy = "vassalCount",
            [FromQuery] string sortOrder = "asc"
        )
        {
            var domains = _context.Domains;
            var selector = GetSelector(sortBy);

            var desc = sortOrder?.ToLower() == "desc";
            var query = desc
                ? domains.OrderByDescending(selector)
                : domains.OrderBy(selector);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return items
                .Select(d => d.ToApi())
                .ToArray();
        }

        private Expression<Func<Organization, object>> GetSelector(string sortBy)
        {
            return sortBy switch
            {
                FactionOrderBy.Name => o => o.Name,
                FactionOrderBy.WarriorCount => o => o.Units.Sum(u => u.Warriors),
                FactionOrderBy.Gold => o => o.Gold,
                FactionOrderBy.Investments => o => o.Investments,
                FactionOrderBy.Fortifications => o => o.Fortifications,
                FactionOrderBy.Suzerain => o => o.Suzerain == null ? "" : o.Suzerain.Name,
                FactionOrderBy.User => o => o.User == null ? "" : o.User.UserName,
                _ => o => o.Vassals.Count,
            };
        }
    }
}
