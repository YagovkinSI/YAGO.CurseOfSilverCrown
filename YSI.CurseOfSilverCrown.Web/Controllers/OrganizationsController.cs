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
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.MainModels.Errors;
using YSI.CurseOfSilverCrown.Core.MainModels.Users;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

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

            ViewBag.CanTake = currentUser != null && currentUser.PersonId == null;
            var doamins = GetDomainsOrderByColumn(_context, column);
            return View(await doamins);
        }

        private async Task<List<Domain>> GetDomainsOrderByColumn(ApplicationDbContext context, int? column)
        {
            var domains = _context.Domains;
            IOrderedQueryable<Domain> orderedDomains = null;
            switch (column)
            {
                case 2:
                    return domains
                        .ToList()
                        .OrderByDescending(o => o.WarriorCount)
                        .ToList();
                case 3:
                    orderedDomains = domains.OrderByDescending(o => o.Coffers);
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
                case 7:
                    orderedDomains = domains.OrderByDescending(o => o.Vassals.Count);
                    break;
                case 8:
                    orderedDomains = domains.OrderBy(o => o.Person.User == null ? "" : o.Person.User.UserName);
                    break;
                case 1:
                default:
                    orderedDomains = domains.OrderBy(o => o.Name);
                    break;
            }
            return await orderedDomains.ToListAsync();
        }

        // GET: Organizations/My
        [Authorize]
        public async Task<IActionResult> My()
        {
            try
            {
                var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

                if (currentUser == null)
                    return NotFound();
                if (currentUser.PersonId == null)
                    return RedirectToAction("Index");

                var organisations = _context.Domains
                    .Where(o => o.PersonId == currentUser.PersonId);

                return View(organisations);
            }
            catch (Exception ex)
            {
                var error = new Error
                {
                    Message = ex.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    StackTrace = ex.StackTrace,
                    TypeFullName = ex.GetType()?.FullName
                };
                _context.Add(error);
                _context.SaveChanges();
            }

            return NotFound();
        }

        // GET: Organizations/Leave
        public async Task<IActionResult> LeaveAsync()
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();
            currentUser.PersonId = null;
            _context.Update(currentUser);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var currentTurn = await _context.Turns.SingleAsync(t => t.IsActive);

            var organisation = await _context.Domains
                .FindAsync(id);

            var organizationEventStories = await _context.OrganizationEventStories
                .Where(o => o.DomainId == organisation.Id && o.TurnId >= currentTurn.Id - 3)
                .ToListAsync();

            var eventStories = organizationEventStories
                .Select(o => o.EventStory)
                .OrderByDescending(o => o.Id)
                .OrderByDescending(o => o.TurnId)
                .ToList();

            ViewBag.LastEventStories = await EventStoryHelper.GetTextStories(_context, eventStories);

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

            if (currentUser.PersonId != null)
                return NotFound();

            var domain = _context.Domains
                .Find(id.Value);

            if (domain.Person.User != null)
                return NotFound();

            currentUser.PersonId = domain.PersonId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
