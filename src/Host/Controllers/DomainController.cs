using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Database.Models.Errors;
using YAGO.World.Infrastructure.Helpers.Events;
using YAGO.World.Infrastructure.Database;

namespace YAGO.World.Host.Controllers
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
                if (!currentUser.Domains.Any())
                    return RedirectToAction("Index", "Organizations");

                var organisation = await _context.Domains
                    .SingleAsync(o => o.UserId == currentUser.Id &&
                        (!domainId.HasValue || o.Id == domainId.Value));

                ViewBag.LastEventStories = await EventHelper.GetTopHistory(_context, domainId ?? organisation.Id);

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
