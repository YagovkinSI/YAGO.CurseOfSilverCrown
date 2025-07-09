using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Helpers.Events;

namespace YAGO.World.Host.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public OrganizationsController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Organizations/Leave
        public async Task<IActionResult> LeaveAsync()
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();

            var domain = currentUser.Domains.SingleOrDefault();
            if (domain != null)
            {
                domain.UserId = null;
                _context.Update(domain);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var organisation = await _context.Domains
                .FindAsync(id);

            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            ViewBag.CanTake = currentUser != null && !currentUser.Domains.Any();
            ViewBag.LastEventStories = await EventHelper.GetTopHistory(_context, id);

            return View(organisation);
        }

        // GET: Organizations/Take/5
        public async Task<IActionResult> Take(int? id)
        {
            if (id == null)
                return NotFound();

            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            if (currentUser == null)
                return NotFound();

            if (currentUser.Domains.Any())
                return NotFound();

            var domain = _context.Domains
                .Find(id.Value);

            if (domain.User != null)
                return NotFound();

            domain.UserId = currentUser.Id;
            _context.Update(domain);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Domain");
        }
    }
}
