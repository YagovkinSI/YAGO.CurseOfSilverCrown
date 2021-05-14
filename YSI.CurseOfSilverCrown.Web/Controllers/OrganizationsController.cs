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

        // GET: Organizations/My
        [Authorize]
        public async Task<IActionResult> My()
        {
            try
            {
                var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

                if (currentUser == null)
                    return NotFound();
                if (string.IsNullOrEmpty(currentUser.OrganizationId))
                    return RedirectToAction("Index", "Provinces");

                var organisation = await _context.Organizations
                    .Include(o => o.User)
                    .Include(o => o.Suzerain)
                    .Include(o => o.Vassals)
                    .Include(o => o.Commands)
                    .Include("Commands.Target")
                    .SingleAsync(o => o.Id == currentUser.OrganizationId);

                var currentTurn = await _context.Turns.SingleAsync(t => t.IsActive);

                var organizationEventStories = await _context.OrganizationEventStories
                    .Include(o => o.EventStory)
                    .Include("EventStory.Turn")
                    .Where(o => o.OrganizationId == organisation.Id && o.TurnId >= currentTurn.Id - 3)
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

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var currentTurn = await _context.Turns.SingleAsync(t => t.IsActive);

            var organisation = await _context.Organizations
                .Include(o => o.User)
                .Include(o => o.Province)
                .Include(o => o.Suzerain)
                .Include(o => o.Vassals)
                .SingleAsync(o => o.Id == id);

            var organizationEventStories = await _context.OrganizationEventStories
                .Include(o => o.EventStory)
                .Include("EventStory.Turn")
                .Where(o => o.OrganizationId == organisation.Id && o.TurnId >= currentTurn.Id - 3)
                .ToListAsync();

            var eventStories = organizationEventStories
                .Select(o => o.EventStory)
                .OrderByDescending(o => o.Id)
                .OrderByDescending(o => o.TurnId)
                .ToList();

            ViewBag.LastEventStories = await EventStoryHelper.GetTextStories(_context, eventStories);

            return View(organisation);
        }
    }
}
