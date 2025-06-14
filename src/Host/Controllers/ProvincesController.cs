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
using YAGO.World.Infrastructure.Helpers;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/factions")]
    public class ProvincesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public ProvincesController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<FactionOnList[]> Index(int? column = null)
        {
            //var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            //ViewBag.CanTake = currentUser != null && !currentUser.Domains.Any();

            var doamins = await GetDomainsOrderByColumn(column);
            return doamins
                .Select(d => d.ToApi())
                .ToArray();
        }

        private async Task<List<Organization>> GetDomainsOrderByColumn(int? column)
        {
            var domains = _context.Domains;
            IOrderedQueryable<Organization> orderedDomains = null;
            switch (column)
            {
                case 1:
                    orderedDomains = domains.OrderBy(o => o.Name);
                    break;
                case 2:
                    return domains
                        .ToList()
                        .OrderByDescending(o => o.WarriorCount)
                        .ToList();
                case 3:
                    orderedDomains = domains.OrderByDescending(o => o.Gold);
                    break;
                case 4:
                    orderedDomains = domains.OrderByDescending(o => o.Investments);
                    break;
                case 5:
                    orderedDomains = domains.OrderByDescending(o => o.Fortifications);
                    break;
                case 6:
                    orderedDomains = domains.OrderBy(o => o.Suzerain == null ? "" : o.Suzerain.Name);
                    break;
                case 8:
                    orderedDomains = domains.OrderBy(o => o.User == null ? "" : o.User.UserName);
                    break;
                default:
                    orderedDomains = domains.OrderByDescending(o => o.Vassals.Count);
                    break;
            }
            return await orderedDomains.ToListAsync();
        }
    }
}
