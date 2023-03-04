using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Users;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Database.Errors;

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
                var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

                if (currentUser == null)
                    return NotFound();
                if (currentUser.PersonId == null)
                    return RedirectToAction("Index", "Organizations");

                var organisation = await _context.Domains
                    .SingleAsync(o => o.PersonId == currentUser.PersonId &&
                        (!domainId.HasValue || o.Id == domainId.Value));

                var currentTurn = await _context.Turns.SingleAsync(t => t.IsActive);

                var organizationEventStories =
                    await _context.OrganizationEventStories
                        .Where(o => o.DomainId == organisation.Id && o.TurnId >= currentTurn.Id - 3)
                        .ToListAsync();

                var eventStories = organizationEventStories
                    .Select(o => o.EventStory)
                    .OrderByDescending(o => o.Id)
                    .OrderByDescending(o => o.TurnId)
                    .ToList();

                ViewBag.LastEventStories = await EventHelper.GetTextStories(_context, eventStories);

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
    }
}
