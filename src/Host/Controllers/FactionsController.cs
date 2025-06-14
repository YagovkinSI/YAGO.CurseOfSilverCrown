using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Host.Extensions;
using YAGO.World.Host.Models;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/factions")]
    public class FactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public FactionsController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<FactionOnList[]> Index(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortBy = "vassalCount",
            [FromQuery] string sortOrder = "asc"
        )
        {
            // Сортировка
            var desc = sortOrder?.ToLower() == "desc";
            var doamins = await GetDomainsOrderByColumn(sortBy, desc);

            // Пагинация
            var items = await doamins
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return items
                .Select(d => d.ToApi())
                .ToArray();

            //var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            //ViewBag.CanTake = currentUser != null && !currentUser.Domains.Any();
        }

        private async Task<IOrderedQueryable<Organization>> GetDomainsOrderByColumn(string sortBy, bool desc)
        {
            var domains = _context.Domains;
            IOrderedQueryable<Organization> orderedDomains = null;
            switch (sortBy)
            {
                case "name":
                    orderedDomains = desc
                        ? domains.OrderByDescending(o => o.Name)
                        : domains.OrderBy(o => o.Name);
                    break;
                case "warriorCount":
                    orderedDomains = desc
                        ? domains.OrderByDescending(o => o.WarriorCount)
                        : domains.OrderBy(o => o.WarriorCount);
                    break;
                case "gold":
                    orderedDomains = desc
                        ? domains.OrderByDescending(o => o.Gold)
                        : domains.OrderBy(o => o.Gold);
                    break;
                case "investments":
                    orderedDomains = desc
                        ? domains.OrderByDescending(o => o.Investments)
                        : domains.OrderBy(o => o.Investments);
                    break;
                case "fortifications":
                    orderedDomains = desc
                        ? domains.OrderByDescending(o => o.Fortifications)
                        : domains.OrderBy(o => o.Fortifications);
                    break;
                case "suzerain":
                    orderedDomains = desc
                        ? domains.OrderByDescending(o => o.Suzerain == null ? "" : o.Suzerain.Name)
                        : domains.OrderBy(o => o.Suzerain == null ? "" : o.Suzerain.Name);
                    break;
                case "user":
                    orderedDomains = desc
                        ? domains.OrderByDescending(o => o.User == null ? "" : o.User.UserName)
                        : domains.OrderBy(o => o.User == null ? "" : o.User.UserName);
                    break;
                default:
                    orderedDomains = desc
                        ? domains.OrderByDescending(o => o.Vassals.Count)
                        : domains.OrderBy(o => o.Vassals.Count);
                    break;
            }
            return orderedDomains;
        }
    }
}
