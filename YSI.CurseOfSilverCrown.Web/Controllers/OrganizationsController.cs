using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Users;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Database.Errors;
using YSI.CurseOfSilverCrown.Core.Helpers.Events;

namespace YSI.CurseOfSilverCrown.Web.Controllers
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

        // GET: Organizations
        public async Task<IActionResult> Index(int? column = null)
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            ViewBag.CanTake = currentUser != null && !currentUser.Domains.Any();
            var doamins = DomainHelper.GetDomainsOrderByColumn(_context, column);
            return View(await doamins);
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

            return RedirectToAction(nameof(Index));
        }
    }
}
