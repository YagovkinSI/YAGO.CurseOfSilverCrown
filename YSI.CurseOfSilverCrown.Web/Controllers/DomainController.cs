using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class DomainController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public DomainController(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync(int? domainId)
        {
            try
            {
                User currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

                if (currentUser == null)
                    return NotFound();
                if (currentUser.PersonId == null)
                    return RedirectToAction("Index");

                Domain organisation = await _context.Domains
                    .Include(o => o.Person)
                    .Include("Person.User")
                    .Include(o => o.Suzerain)
                    .Include(o => o.Vassals)
                    .Include(o => o.Commands)
                    .Include("Commands.Target")
                    .Include(o => o.Units)
                    .Include("Units.Target")
                    .SingleAsync(o => o.PersonId == currentUser.PersonId &&
                        (!domainId.HasValue || o.Id == domainId.Value));

                Turn currentTurn = await _context.Turns.SingleAsync(t => t.IsActive);

                System.Collections.Generic.List<DomainEventStory> organizationEventStories = await _context.OrganizationEventStories
                    .Include(o => o.EventStory)
                    .Include("EventStory.Turn")
                    .Where(o => o.DomainId == organisation.Id && o.TurnId >= currentTurn.Id - 3)
                    .ToListAsync();

                System.Collections.Generic.List<EventStory> eventStories = organizationEventStories
                    .Select(o => o.EventStory)
                    .OrderByDescending(o => o.Id)
                    .OrderByDescending(o => o.TurnId)
                    .ToList();

                ViewBag.LastEventStories = await EventStoryHelper.GetTextStories(_context, eventStories);

                return View(organisation);
            }
            catch (Exception ex)
            {
                Error error = new Error
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
    }
}
