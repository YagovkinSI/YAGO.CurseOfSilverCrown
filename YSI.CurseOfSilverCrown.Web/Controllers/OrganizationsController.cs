using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using System.Diagnostics;
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
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            ViewBag.CanTake = currentUser != null && currentUser.PersonId == null;
            return View(await _context.Domains
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals)
                .Include(o => o.Person)
                .Include("Person.User")
                .Include(o => o.Units)
                .OrderBy(o => o.Name)
                .ToListAsync());
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
                    .Include(o => o.Suzerain)
                    .Include(o => o.Vassals)
                    .Include(o => o.Person)
                    .Include("Person.User")
                    .Include(o => o.Units)
                    .OrderBy(o => o.Name)
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

        // GET: Organizations/My
        [Authorize]
        public async Task<IActionResult> MyDomain(int? domainId)
        {
            try
            {
                var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

                if (currentUser == null)
                    return NotFound();
                if (currentUser.PersonId == null)
                    return RedirectToAction("Index");

                var organisation = await _context.Domains
                    .Include(o => o.Person)
                    .Include("Person.User")
                    .Include(o => o.Suzerain)
                    .Include(o => o.Vassals)
                    .Include(o => o.Commands)
                    .Include("Commands.Target")
                    .Include(o => o.Units)
                    .Include("Units.Target")
                    .SingleAsync(o => o.PersonId == currentUser.PersonId && o.Id == domainId);

                var currentTurn = await _context.Turns.SingleAsync(t => t.IsActive);

                var organizationEventStories = await _context.OrganizationEventStories
                    .Include(o => o.EventStory)
                    .Include("EventStory.Turn")
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
                .Include(o => o.Person)
                .Include("Person.User")
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals)
                .Include(o => o.Units)
                .SingleAsync(o => o.Id == id);

            var organizationEventStories = await _context.OrganizationEventStories
                .Include(o => o.EventStory)
                .Include("EventStory.Turn")
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
                .Include(d => d.Person)
                .Include("Person.User")
                .Single(d => d.Id == id.Value);

            if (domain.Person.User != null)
                return NotFound();

            currentUser.PersonId = domain.PersonId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
